using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SkyGlobal.Data;
using SkyGlobal.Models;

namespace SkyGlobal.Controllers
{
    public class LeaveStatusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeaveStatusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LeaveStatus
        public async Task<IActionResult> Index()
        {
            return View(await _context.LeaveStatuses.ToListAsync());
        }

        // GET: LeaveStatus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveStatus = await _context.LeaveStatuses
                .FirstOrDefaultAsync(m => m.LeaveStatusId == id);
            if (leaveStatus == null)
            {
                return NotFound();
            }

            return View(leaveStatus);
        }

        // GET: LeaveStatus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LeaveStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LeaveStatusId,Status")] LeaveStatus leaveStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(leaveStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(leaveStatus);
        }

        // GET: LeaveStatus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveStatus = await _context.LeaveStatuses.FindAsync(id);
            if (leaveStatus == null)
            {
                return NotFound();
            }
            return View(leaveStatus);
        }

        // POST: LeaveStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LeaveStatusId,Status")] LeaveStatus leaveStatus)
        {
            if (id != leaveStatus.LeaveStatusId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leaveStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveStatusExists(leaveStatus.LeaveStatusId))
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
            return View(leaveStatus);
        }

        // GET: LeaveStatus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveStatus = await _context.LeaveStatuses
                .FirstOrDefaultAsync(m => m.LeaveStatusId == id);
            if (leaveStatus == null)
            {
                return NotFound();
            }

            return View(leaveStatus);
        }

        // POST: LeaveStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveStatus = await _context.LeaveStatuses.FindAsync(id);
            if (leaveStatus != null)
            {
                _context.LeaveStatuses.Remove(leaveStatus);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveStatusExists(int id)
        {
            return _context.LeaveStatuses.Any(e => e.LeaveStatusId == id);
        }
    }
}
