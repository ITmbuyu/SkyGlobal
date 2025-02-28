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
    public class EventCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EventCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.EventCategories.ToListAsync());
        }

        // GET: EventCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventCategorie = await _context.EventCategories
                .FirstOrDefaultAsync(m => m.EventCategorieId == id);
            if (eventCategorie == null)
            {
                return NotFound();
            }

            return View(eventCategorie);
        }

        // GET: EventCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EventCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventCategorieId,EventCategorieName")] EventCategorie eventCategorie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventCategorie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eventCategorie);
        }

        // GET: EventCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventCategorie = await _context.EventCategories.FindAsync(id);
            if (eventCategorie == null)
            {
                return NotFound();
            }
            return View(eventCategorie);
        }

        // POST: EventCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventCategorieId,EventCategorieName")] EventCategorie eventCategorie)
        {
            if (id != eventCategorie.EventCategorieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventCategorie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventCategorieExists(eventCategorie.EventCategorieId))
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
            return View(eventCategorie);
        }

        // GET: EventCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventCategorie = await _context.EventCategories
                .FirstOrDefaultAsync(m => m.EventCategorieId == id);
            if (eventCategorie == null)
            {
                return NotFound();
            }

            return View(eventCategorie);
        }

        // POST: EventCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventCategorie = await _context.EventCategories.FindAsync(id);
            if (eventCategorie != null)
            {
                _context.EventCategories.Remove(eventCategorie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventCategorieExists(int id)
        {
            return _context.EventCategories.Any(e => e.EventCategorieId == id);
        }
    }
}
