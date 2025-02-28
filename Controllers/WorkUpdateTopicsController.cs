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
    public class WorkUpdateTopicsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public WorkUpdateTopicsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: WorkUpdateTopics
        public async Task<IActionResult> Index()
        {
            return View(await _context.WorkUpdateTopics.ToListAsync());
        }

        // GET: WorkUpdateTopics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workUpdateTopic = await _context.WorkUpdateTopics
                .FirstOrDefaultAsync(m => m.WorkUpdateTopicId == id);
            if (workUpdateTopic == null)
            {
                return NotFound();
            }

            return View(workUpdateTopic);
        }

        // GET: WorkUpdateTopics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WorkUpdateTopics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WorkUpdateTopicId,Topic,Topicpicture")] WorkUpdateTopic workUpdateTopic, IFormFile Topicpicture)
        {

            if (Topicpicture != null && Topicpicture.Length > 0)
            {
                // Generate a unique file name for the image (you can customize this logic)
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Topicpicture.FileName;

                // Define the path to save the image in the wwwroot/uploads folder
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Create the uploads folder if it doesn't exist
                Directory.CreateDirectory(uploadsFolder);

                // Save the uploaded image to the file system
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Topicpicture.CopyToAsync(stream);
                }

                // Save the image file path to the database
                workUpdateTopic.Topicpicture = "/uploads/" + uniqueFileName; // Relative path to the image

                _context.Add(workUpdateTopic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            if (ModelState.IsValid)
            {
                _context.Add(workUpdateTopic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(workUpdateTopic);
        }

        // GET: WorkUpdateTopics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workUpdateTopic = await _context.WorkUpdateTopics.FindAsync(id);
            if (workUpdateTopic == null)
            {
                return NotFound();
            }
            return View(workUpdateTopic);
        }

        // POST: WorkUpdateTopics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WorkUpdateTopicId,Topic,Topicpicture")] WorkUpdateTopic workUpdateTopic)
        {
            if (id != workUpdateTopic.WorkUpdateTopicId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workUpdateTopic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkUpdateTopicExists(workUpdateTopic.WorkUpdateTopicId))
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
            return View(workUpdateTopic);
        }

        // GET: WorkUpdateTopics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workUpdateTopic = await _context.WorkUpdateTopics
                .FirstOrDefaultAsync(m => m.WorkUpdateTopicId == id);
            if (workUpdateTopic == null)
            {
                return NotFound();
            }

            return View(workUpdateTopic);
        }

        // POST: WorkUpdateTopics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workUpdateTopic = await _context.WorkUpdateTopics.FindAsync(id);
            if (workUpdateTopic != null)
            {
                _context.WorkUpdateTopics.Remove(workUpdateTopic);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkUpdateTopicExists(int id)
        {
            return _context.WorkUpdateTopics.Any(e => e.WorkUpdateTopicId == id);
        }
    }
}
