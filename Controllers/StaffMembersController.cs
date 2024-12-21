﻿using System;
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
    public class StaffMembersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StaffMembersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StaffMembers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.StaffMembers.Include(s => s.Team);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: StaffMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staffMember = await _context.StaffMembers
                .Include(s => s.Team)
                .FirstOrDefaultAsync(m => m.StaffMemberId == id);
            if (staffMember == null)
            {
                return NotFound();
            }

            return View(staffMember);
        }

        // GET: StaffMembers/Create
        public IActionResult Create()
        {
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "TeamId");
            return View();
        }

        // POST: StaffMembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StaffMemberId,StaffMemberName,StaffMemberSurname,StaffMemberEmail,StaffMemberPhone,StaffMemberPassword,StaffMemberRole,StaffMemeberRate,StaffMemeberHours,TeamId")] StaffMember staffMember)
        {
            if (ModelState.IsValid)
            {
                _context.Add(staffMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "TeamId", staffMember.TeamId);
            return View(staffMember);
        }

        // GET: StaffMembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staffMember = await _context.StaffMembers.FindAsync(id);
            if (staffMember == null)
            {
                return NotFound();
            }
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "TeamId", staffMember.TeamId);
            return View(staffMember);
        }

        // POST: StaffMembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StaffMemberId,StaffMemberName,StaffMemberSurname,StaffMemberEmail,StaffMemberPhone,StaffMemberPassword,StaffMemberRole,StaffMemeberRate,StaffMemeberHours,TeamId")] StaffMember staffMember)
        {
            if (id != staffMember.StaffMemberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(staffMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffMemberExists(staffMember.StaffMemberId))
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
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "TeamId", staffMember.TeamId);
            return View(staffMember);
        }

        // GET: StaffMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staffMember = await _context.StaffMembers
                .Include(s => s.Team)
                .FirstOrDefaultAsync(m => m.StaffMemberId == id);
            if (staffMember == null)
            {
                return NotFound();
            }

            return View(staffMember);
        }

        // POST: StaffMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var staffMember = await _context.StaffMembers.FindAsync(id);
            if (staffMember != null)
            {
                _context.StaffMembers.Remove(staffMember);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StaffMemberExists(int id)
        {
            return _context.StaffMembers.Any(e => e.StaffMemberId == id);
        }
    }
}