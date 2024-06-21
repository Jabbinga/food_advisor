using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Licencjat.Data;
using Licencjat.Models;
using Microsoft.AspNetCore.Authorization;

namespace Licencjat.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class DishTagController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DishTagController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DishTag
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DishTags.Include(d => d.Dish).Include(d => d.Tag);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DishTag/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishTag = await _context.DishTags
                .Include(d => d.Dish)
                .Include(d => d.Tag)
                .FirstOrDefaultAsync(m => m.DishId == id);
            if (dishTag == null)
            {
                return NotFound();
            }

            return View(dishTag);
        }

        // GET: DishTag/Create
        public IActionResult Create()
        {
            ViewData["DishId"] = new SelectList(_context.Dish, "Id", "Name");
            ViewData["TagId"] = new SelectList(_context.Tag, "Id", "Name");
            return View();
        }

        // POST: DishTag/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DishId,TagId")] DishTag dishTag)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dishTag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DishId"] = new SelectList(_context.Dish, "Id", "Name", dishTag.DishId);
            ViewData["TagId"] = new SelectList(_context.Tag, "Id", "Name", dishTag.TagId);
            return View(dishTag);
        }

        // GET: DishTag/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishTag = await _context.DishTags.FindAsync(id);
            if (dishTag == null)
            {
                return NotFound();
            }
            ViewData["DishId"] = new SelectList(_context.Dish, "Id", "Name", dishTag.DishId);
            ViewData["TagId"] = new SelectList(_context.Tag, "Id", "Name", dishTag.TagId);
            return View(dishTag);
        }

        // POST: DishTag/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DishId,TagId")] DishTag dishTag)
        {
            if (id != dishTag.DishId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dishTag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishTagExists(dishTag.DishId))
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
            ViewData["DishId"] = new SelectList(_context.Dish, "Id", "Name", dishTag.DishId);
            ViewData["TagId"] = new SelectList(_context.Tag, "Id", "Name", dishTag.TagId);
            return View(dishTag);
        }

        // GET: DishTag/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishTag = await _context.DishTags
                .Include(d => d.Dish)
                .Include(d => d.Tag)
                .FirstOrDefaultAsync(m => m.DishId == id);
            if (dishTag == null)
            {
                return NotFound();
            }

            return View(dishTag);
        }

        // POST: DishTag/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dishTag = await _context.DishTags.FindAsync(id);
            if (dishTag != null)
            {
                _context.DishTags.Remove(dishTag);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishTagExists(int id)
        {
            return _context.DishTags.Any(e => e.DishId == id);
        }
    }
}
