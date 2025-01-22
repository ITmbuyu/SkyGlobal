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

namespace SkyGlobal.Controllers
{
    public class AttendanceBonusController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AttendanceBonusController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: AttendanceBonus
        public async Task<IActionResult> Index(string month)
        {
            var bonusesQuery = _context.AttendanceBonuses.AsQueryable();

            if (!string.IsNullOrEmpty(month))
            {
                var selectedMonth = DateTime.ParseExact(month, "yyyy-MM", System.Globalization.CultureInfo.InvariantCulture);
                bonusesQuery = bonusesQuery.Where(ab => ab.Month.Year == selectedMonth.Year && ab.Month.Month == selectedMonth.Month);
            }

            var bonuses = await bonusesQuery.ToListAsync();
            ViewData["SelectedMonth"] = month;

            return View(bonuses);
        }

        // Calculate attendance bonus for a specific user and month
        public async Task<IActionResult> CalculateBonus(string userId, DateTime month)
        {
            // Fetch attendance records for the given month and user
            var attendanceRecords = await _context.AttendanceRecords
                .Where(a => a.UserId == userId && a.Date.Month == month.Month && a.Date.Year == month.Year)
                .ToListAsync();

            // Fetch approved leave requests for the given month and user
            var approvedLeaves = await _context.LeaveRequests
                .Include(l => l.LeaveStatus) // Include LeaveStatus for filtering
                .Where(l => l.UserId == userId &&
                            l.LeaveStatus != null && l.LeaveStatus.Status == "Approved" &&
                            l.StartDate.Month <= month.Month && l.EndDate.Month >= month.Month &&
                            l.StartDate.Year <= month.Year && l.EndDate.Year >= month.Year)
                .ToListAsync();

            // Calculate total working days and presence
            int totalWorkingDays = DateTime.DaysInMonth(month.Year, month.Month);
            int presentDays = attendanceRecords.Count(a => a.Status == "Present");
            int leaveDays = approvedLeaves.Sum(l => (l.EndDate - l.StartDate).Days + 1); // Inclusive of both start and end

            // Determine bonus eligibility
            decimal bonusAmount = (presentDays + leaveDays) >= totalWorkingDays ? 1400 : 0;

            // Update or create attendance bonus record
            var attendanceBonus = await _context.AttendanceBonuses
                .FirstOrDefaultAsync(ab => ab.UserId == userId && ab.Month.Month == month.Month && ab.Month.Year == month.Year);

            if (attendanceBonus == null)
            {
                // Create new record
                attendanceBonus = new AttendanceBonus
                {
                    UserId = userId,
                    Month = new DateTime(month.Year, month.Month, 1),
                    Amount = bonusAmount
                };
                _context.AttendanceBonuses.Add(attendanceBonus);
            }
            else
            {
                // Update existing record
                attendanceBonus.Amount = bonusAmount;
                _context.AttendanceBonuses.Update(attendanceBonus);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Helper: Calculate bonus for all advisors for a given month
        public async Task<IActionResult> CalculateBonusForAll(DateTime month)
        {
            var advisors = await _userManager.GetUsersInRoleAsync("Advisor");

            foreach (var advisor in advisors)
            {
                await CalculateBonus(advisor.Id, month);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendanceBonus = await _context.AttendanceBonuses
                .FirstOrDefaultAsync(m => m.AttendanceBonusId == id);
            if (attendanceBonus == null)
            {
                return NotFound();
            }

            return View(attendanceBonus);
        }

        private bool AttendanceBonusExists(int id)
        {
            return _context.AttendanceBonuses.Any(e => e.AttendanceBonusId == id);
        }
    }
}
