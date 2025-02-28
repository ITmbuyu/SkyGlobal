using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SkyGlobal.Data;
using SkyGlobal.Models;
using System.Net.Mail;
using System.Net;
using Azure.Core;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.UI.Services;
using NuGet.Protocol.Plugins;

namespace SkyGlobal.Controllers
{
    public class LeaveRequestsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;

        public LeaveRequestsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            this._emailSender = emailSender; // Inject email sender

        }

        // GET: LeaveRequests
        public async Task<IActionResult> Index()
        {
            //var receiver = "@gmail.com";
            //var subject = "Test";
            //var message = "Hello World";

            //await _emailSender.SendEmailAsync(receiver, subject, message);

            var applicationDbContext = _context.LeaveRequests.Include(l => l.LeaveStatus).Include(l => l.LeaveType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: LeaveRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveRequest = await _context.LeaveRequests
                .Include(l => l.LeaveStatus)
                .Include(l => l.LeaveType)
                .FirstOrDefaultAsync(m => m.LeaveRequestId == id);
            if (leaveRequest == null)
            {
                return NotFound();
            }

            return View(leaveRequest);
        }



        // GET: LeaveRequests/Create
        public IActionResult Create()
        {

            ViewData["LeaveStatusId"] = new SelectList(_context.LeaveStatuses, "LeaveStatusId", "Status");
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "LeaveTypeId", "Type");
            return View();
        }

        // POST: LeaveRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LeaveRequestId,UserId,StartDate,EndDate,Type,Status,ApproverId,ApproverRole,RequestDate,ApprovalDate,Comments,LeaveStatusId,LeaveTypeId")] LeaveRequest leaveRequest)
        {
            if (ModelState.IsValid)
            {
                // Get the current logged-in user
                var currentUser = await _userManager.GetUserAsync(User);


                if (currentUser != null)
                {
                    // Automatically set UserId to the logged-in user's ID
                    leaveRequest.UserId = currentUser.Id;

                    // Automatically set the RequestDate
                    leaveRequest.RequestDate = DateTime.Now;

                    // Set default status to "Pending" by finding the corresponding LeaveStatusId
                    leaveRequest.LeaveStatusId = _context.LeaveStatuses
                        .Where(ls => ls.Status == "Pending")
                        .Select(ls => ls.LeaveStatusId)
                        .FirstOrDefault();

                    // Save the leave request to the database
                    _context.Add(leaveRequest);
                    await _context.SaveChangesAsync();

                    // Get admin email
                    //var adminUser = await _userManager.GetUsersInRoleAsync("Admin");
                    //var adminEmail = adminUser.FirstOrDefault()?.Email;

                    // Hard-code the admin email for testing
                    var adminEmail = "your-email@gmail.com";


                    if (!string.IsNullOrEmpty(adminEmail))
                    {
                        string subject = "New Leave Request Submitted";
                        string message = $"A new leave request has been submitted by {currentUser.UserName}.\n\n" +
                                         $"Start Date: {leaveRequest.StartDate}\n" +
                                         $"End Date: {leaveRequest.EndDate}\n" +
                                         $"Type: {leaveRequest.LeaveTypeId}\n\n" +
                                         "Please review the request.";

                        // Send email to admin
                        await _emailSender.SendEmailAsync(adminEmail, subject, message);
                    }


                    // Redirect to the index page or another page
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Handle the case where the user is not found (if necessary)
                    return Unauthorized();
                }
            }
            ViewData["LeaveStatusId"] = new SelectList(_context.LeaveStatuses, "LeaveStatusId", "LeaveStatusId", leaveRequest.LeaveStatusId);
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "LeaveTypeId", "LeaveTypeId", leaveRequest.LeaveTypeId);
            return View(leaveRequest);
        }

        // GET: LeaveRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var leaveRequest = await _context.LeaveRequests.FindAsync(id);
            if (leaveRequest == null)
            {
                return NotFound();
            }
            // Get the current logged-in user
            var currentUser = await _userManager.GetUserAsync(User);
            string userRole = currentUser != null ? await GetRoleForUserAsync(currentUser) : string.Empty;

            // Pass the role to the view
            ViewBag.UserRole = userRole;
            if (currentUser != null && await _userManager.IsInRoleAsync(currentUser, "Admin"))
            {
                // Allow Admin to edit the leave request (ApproverId, ApproverRole, ApprovalDate will be set automatically)
                ViewData["LeaveStatusId"] = new SelectList(_context.LeaveStatuses, "LeaveStatusId", "Status", leaveRequest.LeaveStatusId);
                ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "LeaveTypeId", "Type", leaveRequest.LeaveTypeId);
                return View(leaveRequest);
            }
            else
            {
                // For regular users, you can restrict the approver fields or leave them hidden.
                ViewData["LeaveStatusId"] = new SelectList(_context.LeaveStatuses, "LeaveStatusId", "Status", leaveRequest.LeaveStatusId);
                ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "LeaveTypeId", "Type", leaveRequest.LeaveTypeId);
                return View(leaveRequest);
            }

        }

        private async Task<string?> GetRoleForUserAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.FirstOrDefault(); // Return the first role (or modify logic as needed)
        }


        // POST: LeaveRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LeaveRequestId,UserId,StartDate,EndDate,Type,Status,ApproverId,ApproverRole,RequestDate,ApprovalDate,Comments,LeaveStatusId,LeaveTypeId")] LeaveRequest leaveRequest)
        {
            if (id != leaveRequest.LeaveRequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the current logged-in user
                    var currentUser = await _userManager.GetUserAsync(User);

                    if (currentUser != null)
                    {
                        var role = await GetRoleForUserAsync(currentUser);

                        if (role == "Admin")
                        {
                            // Admin-specific logic
                            leaveRequest.ApproverId = currentUser.Id;
                            leaveRequest.ApproverRole = role; // Use dynamic role
                            leaveRequest.ApprovalDate = DateTime.Now;

                            // Send email notification to the user - hardcoded because... will fix later
                            var user = "your-email@gmail.com";


                            if (user != null)
                            {
                                var subject = "Your Leave Request Status Has Been Updated";
                                var body = $"Dear {user},\n\n" +
                                           $"Your leave request with ID {leaveRequest.LeaveRequestId} has been updated. " +
                                           $"Approval Date: {leaveRequest.ApprovalDate}\n\n" +
                                           $"If you have any questions, please contact the admin.\n\n" +
                                           "Regards,\nYour Company HR";

                                await _emailSender.SendEmailAsync(user, subject, body);
                            }
                        }
                        else
                        {
                            // Regular user logic
                            leaveRequest.ApproverId = null;
                            leaveRequest.ApproverRole = null;
                            leaveRequest.ApprovalDate = null;

                            // Optional: Clear comments if regular users cannot provide them
                            leaveRequest.Comments = null;

                            // Optional: LeaveStatus could be modified here based on business rules
                            leaveRequest.LeaveStatus = leaveRequest.LeaveStatus; // Or modify accordingly
                        }
                    }

                    // Update the LeaveRequest
                    _context.Update(leaveRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveRequestExists(leaveRequest.LeaveRequestId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["LeaveStatusId"] = new SelectList(_context.LeaveStatuses, "LeaveStatusId", "LeaveStatusId", leaveRequest.LeaveStatusId);
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "LeaveTypeId", "LeaveTypeId", leaveRequest.LeaveTypeId);
            return View(leaveRequest);
        }

        // GET: LeaveRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveRequest = await _context.LeaveRequests
                .Include(l => l.LeaveStatus)
                .Include(l => l.LeaveType)
                .FirstOrDefaultAsync(m => m.LeaveRequestId == id);
            if (leaveRequest == null)
            {
                return NotFound();
            }

            return View(leaveRequest);
        }

        // POST: LeaveRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(id);
            if (leaveRequest != null)
            {
                _context.LeaveRequests.Remove(leaveRequest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveRequestExists(int id)
        {
            return _context.LeaveRequests.Any(e => e.LeaveRequestId == id);
        }
    }
}
