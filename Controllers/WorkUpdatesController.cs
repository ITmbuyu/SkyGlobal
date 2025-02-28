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
    public class WorkUpdatesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public WorkUpdatesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> UserManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = UserManager;
        }


        // GET: WorkUpdates
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.WorkUpdates.Include(w => w.WorkUpdateTopic);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: WorkUpdates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workUpdate = await _context.WorkUpdates
                .Include(w => w.WorkUpdateTopic)
                .FirstOrDefaultAsync(m => m.WorkUpdateId == id);
            if (workUpdate == null)
            {
                return NotFound();
            }

            return View(workUpdate);
        }

        // GET: WorkUpdates/Create
        public IActionResult Create()
        {
            ViewData["WorkUpdateTopicId"] = new SelectList(_context.WorkUpdateTopics, "WorkUpdateTopicId", "Topic");
            return View();
        }

        // POST: WorkUpdates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WorkUpdateId,Title,Description,ArticlePicture,Update,Author,WorkUpdateTopicId,TrendingNow,BreakingNews,IsPublished,PickOfMonth,EditorsPick,DatePublished,ReadingDuration,IsApproved")] WorkUpdate workUpdate,IFormFile ArticlePicture)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            workUpdate.Author = _userManager.GetUserName(User);
            // Check if an image was uploaded
            if (ArticlePicture != null && ArticlePicture.Length > 0)
            {
                // Generate a unique file name for the image (you can customize this logic)
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + ArticlePicture.FileName;

                // Define the path to save the image in the wwwroot/uploads folder
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Create the uploads folder if it doesn't exist
                Directory.CreateDirectory(uploadsFolder);

                // Save the uploaded image to the file system
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ArticlePicture.CopyToAsync(stream);
                }

                // Save the image file path to the database
                workUpdate.ArticlePicture = "/uploads/" + uniqueFileName; // Relative path to the image

                //var user = await _userManager.GetUserAsync(User);
                //var email = user.Email;

                //bool isAdmin = currentUser.IsInRole("Admin");
                //var userId = _userManager.GetUserId(User); // Get user ID
            }

            if (ModelState.IsValid)
            {
                _context.Add(workUpdate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["WorkUpdateTopicId"] = new SelectList(_context.WorkUpdateTopics, "WorkUpdateTopicId", "Topic", workUpdate.WorkUpdateTopicId);
            return View(workUpdate);
        }

        // GET: WorkUpdates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workUpdate = await _context.WorkUpdates.FindAsync(id);
            if (workUpdate == null)
            {
                return NotFound();
            }
            ViewData["WorkUpdateTopicId"] = new SelectList(_context.WorkUpdateTopics, "WorkUpdateTopicId", "WorkUpdateTopicId", workUpdate.WorkUpdateTopicId);
            return View(workUpdate);
        }

        // POST: WorkUpdates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WorkUpdateId,Title,Description,ArticlePicture,Update,Author,WorkUpdateTopicId,TrendingNow,BreakingNews,IsPublished,PickOfMonth,EditorsPick,DatePublished,ReadingDuration,IsApproved")] WorkUpdate workUpdate, IFormFile ArticlePicture)
        {
            if (id != workUpdate.WorkUpdateId)
            {
                return NotFound();
            }

            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            workUpdate.Author = _userManager.GetUserName(User);
            // Check if an image was uploaded
            if (ArticlePicture != null && ArticlePicture.Length > 0)
            {
                // Generate a unique file name for the image (you can customize this logic)
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + ArticlePicture.FileName;

                // Define the path to save the image in the wwwroot/uploads folder
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Create the uploads folder if it doesn't exist
                Directory.CreateDirectory(uploadsFolder);

                // Save the uploaded image to the file system
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ArticlePicture.CopyToAsync(stream);
                }

                // Save the image file path to the database
                workUpdate.ArticlePicture = "/uploads/" + uniqueFileName; // Relative path to the image

                //var user = await _userManager.GetUserAsync(User);
                //var email = user.Email;

                //bool isAdmin = currentUser.IsInRole("Admin");
                //var userId = _userManager.GetUserId(User); // Get user ID
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkUpdateExists(workUpdate.WorkUpdateId))
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
            ViewData["WorkUpdateTopicId"] = new SelectList(_context.WorkUpdateTopics, "WorkUpdateTopicId", "WorkUpdateTopicId", workUpdate.WorkUpdateTopicId);
            return View(workUpdate);
        }

        // GET: WorkUpdates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workUpdate = await _context.WorkUpdates
                .Include(w => w.WorkUpdateTopic)
                .FirstOrDefaultAsync(m => m.WorkUpdateId == id);
            if (workUpdate == null)
            {
                return NotFound();
            }

            return View(workUpdate);
        }

        // POST: WorkUpdates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workUpdate = await _context.WorkUpdates.FindAsync(id);
            if (workUpdate != null)
            {
                _context.WorkUpdates.Remove(workUpdate);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkUpdateExists(int id)
        {
            return _context.WorkUpdates.Any(e => e.WorkUpdateId == id);
        }
    }
}
