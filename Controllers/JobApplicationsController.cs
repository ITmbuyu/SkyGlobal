using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SkyGlobal.Data;
using SkyGlobal.Models;
using Microsoft.AspNetCore.Authorization;

namespace SkyGlobal.Controllers
{
    public class JobApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobApplicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Show all job applications
        public async Task<IActionResult> Index()
        {
            var applications = await _context.JobApplications.Include(j => j.Job).ToListAsync();
            return View(applications);
        }

        // Show application form
        public IActionResult Apply(int jobId)
        {
            var job = _context.JobPostings.Find(jobId);
            if (job == null)
                return NotFound();

            ViewBag.JobTitle = job.Title;
            return View(new JobApplication { JobPostingId = jobId });
        }

        // Handle application submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(JobApplication jobApplication, IFormFile resumeFile)
        {
            if (!_context.JobPostings.Any(j => j.JobPostingId == jobApplication.JobPostingId))
            {
                ModelState.AddModelError("", "Selected job does not exist.");
                return View(jobApplication);
            }

            if (resumeFile != null && resumeFile.Length > 0)
            {
                // Define the uploads directory (ensure this folder exists in wwwroot)
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                // Create directory if not exists
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generate unique file name
                var uniqueFileName = $"{Guid.NewGuid()}_{resumeFile.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save file to server
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await resumeFile.CopyToAsync(fileStream);
                }

                // Store file path in database (relative path for web access)
                jobApplication.ResumePath = "/uploads/" + uniqueFileName;
            }
            else
            {
                ModelState.AddModelError("ResumePath", "Resume is required.");
                return View(jobApplication);
            }

            if (ModelState.IsValid)
            {
                jobApplication.AppliedDate = DateTime.Now;
                _context.Add(jobApplication);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "JobPostings");
            }
            return View(jobApplication);
        }



        // View application details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var application = await _context.JobApplications.Include(j => j.Job)
                                .FirstOrDefaultAsync(m => m.ApplicationId == id);

            if (application == null)
                return NotFound();

            return View(application);
        }

        // Delete application confirmation
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var application = await _context.JobApplications.Include(j => j.Job)
                                .FirstOrDefaultAsync(m => m.ApplicationId == id);

            if (application == null)
                return NotFound();

            return View(application);
        }

        // Handle deletion
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var application = await _context.JobApplications.FindAsync(id);
            if (application != null)
            {
                _context.JobApplications.Remove(application);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }

}
