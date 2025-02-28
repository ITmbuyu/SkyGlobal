using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkyGlobal.Data;
using SkyGlobal.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SkyGlobal.Controllers
{
    public class JobPostingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobPostingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Show all job postings
        public async Task<IActionResult> Index(bool? isInternal)
        {
            var jobs = _context.JobPostings.AsQueryable();

            // Filter based on role
            if (User.IsInRole("Admin"))
            {
                // Admin sees everything and can filter internal/external
                if (isInternal.HasValue)
                    jobs = jobs.Where(j => j.IsInternal == isInternal.Value);
            }
            else if (User.IsInRole("Staff") || User.IsInRole("Team Lead"))
            {
                // Staff & Team Lead see only internal jobs
                jobs = jobs.Where(j => j.IsInternal);
            }
            else
            {
                // Regular users see only external jobs
                jobs = jobs.Where(j => !j.IsInternal);
            }

            return View(await jobs.ToListAsync());
        }



        // Show job creation form
        [Authorize(Roles = "Admin, Manager")]
        public IActionResult Create()
        {
            return View();
        }


        // Handle job creation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title, Description, Requirements, Location, IsInternal, ExpiryDate")] JobPosting jobPosting)
        {
            if (ModelState.IsValid)
            {
                jobPosting.PostedDate = DateTime.Now;
                _context.Add(jobPosting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jobPosting);
        }

        // Show job details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var job = await _context.JobPostings.FirstOrDefaultAsync(m => m.JobPostingId == id);

            if (job == null)
                return NotFound();

            return View(job);
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var job = await _context.JobPostings.FindAsync(id);

            if (job == null)
                return NotFound();

            return View(job);
        }

        // Handle job update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("JobPostingId, Title, Description, Requirements, Location, IsInternal, ExpiryDate, PostedDate")] JobPosting jobPosting)
        {
            if (id != jobPosting.JobPostingId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobPosting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.JobPostings.Any(e => e.JobPostingId == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(jobPosting);
        }

        // Show delete confirmation
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var job = await _context.JobPostings.FirstOrDefaultAsync(m => m.JobPostingId == id);

            if (job == null)
                return NotFound();

            return View(job);
        }

        // Handle job deletion
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var job = await _context.JobPostings.FindAsync(id);
            if (job != null)
            {
                _context.JobPostings.Remove(job);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
