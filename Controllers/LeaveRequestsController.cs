using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SkyGlobal.Data;
using SkyGlobal.Models;

namespace SkyGlobal.Controllers
{
    public class LeaveRequestsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        


        public LeaveRequestsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment , UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: LeaveRequests
        public async Task<IActionResult> Index()
        {
            return View(await _context.LeaveRequests.ToListAsync());
        }

        // GET: LeaveRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveRequest = await _context.LeaveRequests
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
            return View();
        }

        // POST: LeaveRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LeaveRequestId,UserId,StartDate,EndDate,Status,ApproverId,ApproverRole,RequestDate,ApprovalDate,Comments")] LeaveRequest leaveRequest)
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

                    // Set default status for the leave request
                    leaveRequest.Status = "Pending";

                    // Save the leave request to the database
                    _context.Add(leaveRequest);
                    await _context.SaveChangesAsync();

                    // Redirect to the index page or another page
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Handle the case where the user is not found (if necessary)
                    return Unauthorized();
                }
            }

            // Return the view with the model if validation failed
            return View(leaveRequest);
        }


        // GET: LeaveRequests/Edit/5
        [HttpGet]
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

            if (currentUser != null && await _userManager.IsInRoleAsync(currentUser, "Admin"))
            {
                // Allow Admin to edit the leave request (ApproverId, ApproverRole, ApprovalDate will be set automatically)
                return View(leaveRequest);
            }
            else
            {
                // For regular users, you can restrict the approver fields or leave them hidden.
                return View(leaveRequest);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LeaveRequestId,UserId,StartDate,EndDate,Status,RequestDate,ApprovalDate,Comments,ApproverId,ApproverRole")] LeaveRequest leaveRequest)
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

                    // If the current user is an Admin, set ApproverId, ApproverRole, and ApprovalDate
                    if (currentUser != null && await _userManager.IsInRoleAsync(currentUser, "Admin"))
                    {
                        // Set the ApproverId to the current user's ID (admin)
                        leaveRequest.ApproverId = currentUser.Id;

                        // Set the ApproverRole to "Admin" or another role if needed
                        leaveRequest.ApproverRole = "Admin";

                        // Set the current date/time as ApprovalDate
                        leaveRequest.ApprovalDate = DateTime.Now;

                        _context.Update(leaveRequest);
                    }
                    else
                    {
                        // For regular users, they cannot set these fields, so clear them.
                        leaveRequest.ApproverId = null;
                        leaveRequest.ApproverRole = null;
                        leaveRequest.ApprovalDate = null;

                        _context.Update(leaveRequest);
                    }

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
