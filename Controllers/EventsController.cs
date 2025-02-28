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
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> UserManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = UserManager;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Events.Include(e => e.EventCategorie);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(e => e.EventCategorie)
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            ViewData["EventCategorieId"] = new SelectList(_context.EventCategories, "EventCategorieId", "EventCategorieName");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,EventName,EventDescription,EventTime,EventDate,EventThumbnail,EventCategorieId")] Event @event, IFormFile EventThumbnail)
        {
            // Check if an image was uploaded
            if (EventThumbnail != null && EventThumbnail.Length > 0)
            {
                // Generate a unique file name for the image (you can customize this logic)
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + EventThumbnail.FileName;

                // Define the path to save the image in the wwwroot/uploads folder
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Create the uploads folder if it doesn't exist
                Directory.CreateDirectory(uploadsFolder);

                // Save the uploaded image to the file system
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await EventThumbnail.CopyToAsync(stream);
                }

                // Save the image file path to the database
                @event.EventThumbnail = "/uploads/" + uniqueFileName; // Relative path to the image

                //var user = await _userManager.GetUserAsync(User);
                //var email = user.Email;

                //bool isAdmin = currentUser.IsInRole("Admin");
                //var userId = _userManager.GetUserId(User); // Get user ID
            }

            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventCategorieId"] = new SelectList(_context.EventCategories, "EventCategorieId", "EventCategorieName", @event.EventCategorieId);
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            ViewData["EventCategorieId"] = new SelectList(_context.EventCategories, "EventCategorieId", "EventCategorieName", @event.EventCategorieId);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,EventName,EventDescription,EventTime,EventDate,EventThumbnail,EventCategorieId")] Event @event,IFormFile EventThumbnail)
        {
            if (id != @event.EventId)
            {
                return NotFound();
            }

            // Check if an image was uploaded
            if (EventThumbnail != null && EventThumbnail.Length > 0)
            {
                // Generate a unique file name for the image (you can customize this logic)
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + EventThumbnail.FileName;

                // Define the path to save the image in the wwwroot/uploads folder
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Create the uploads folder if it doesn't exist
                Directory.CreateDirectory(uploadsFolder);

                // Save the uploaded image to the file system
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await EventThumbnail.CopyToAsync(stream);
                }

                // Save the image file path to the database
                @event.EventThumbnail = "/uploads/" + uniqueFileName; // Relative path to the image

                //var user = await _userManager.GetUserAsync(User);
                //var email = user.Email;

                //bool isAdmin = currentUser.IsInRole("Admin");
                //var userId = _userManager.GetUserId(User); // Get user ID
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.EventId))
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
            ViewData["EventCategorieId"] = new SelectList(_context.EventCategories, "EventCategorieId", "EventCategorieName", @event.EventCategorieId);
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(e => e.EventCategorie)
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                _context.Events.Remove(@event);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }
    }
}
