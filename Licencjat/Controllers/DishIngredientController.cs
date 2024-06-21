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
    public class DishIngredientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DishIngredientController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DishIngredient
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DishIngredient.Include(d => d.Dish).Include(d => d.Ingredient);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DishIngredient/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishIngredient = await _context.DishIngredient
                .Include(d => d.Dish)
                .Include(d => d.Ingredient)
                .FirstOrDefaultAsync(m => m.DishId == id);
            if (dishIngredient == null)
            {
                return NotFound();
            }

            return View(dishIngredient);
        }

        // GET: DishIngredient/Create
        public IActionResult Create()
        {
            ViewData["DishId"] = new SelectList(_context.Dish, "Id", "Name");
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "Id", "Name");
            return View();
        }

        // POST: DishIngredient/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DishId,IngredientId")] DishIngredient dishIngredient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dishIngredient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DishId"] = new SelectList(_context.Dish, "Id", "Name", dishIngredient.DishId);
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "Id", "Name", dishIngredient.IngredientId);
            return View(dishIngredient);
        }

        // GET: DishIngredient/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishIngredient = await _context.DishIngredient.FindAsync(id);
            if (dishIngredient == null)
            {
                return NotFound();
            }
            ViewData["DishId"] = new SelectList(_context.Dish, "Id", "Name", dishIngredient.DishId);
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "Id", "Name", dishIngredient.IngredientId);
            return View(dishIngredient);
        }

        // POST: DishIngredient/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DishId,IngredientId")] DishIngredient dishIngredient)
        {
            if (id != dishIngredient.DishId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dishIngredient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishIngredientExists(dishIngredient.DishId))
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
            ViewData["DishId"] = new SelectList(_context.Dish, "Id", "Name", dishIngredient.DishId);
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "Id", "Name", dishIngredient.IngredientId);
            return View(dishIngredient);
        }

        // GET: DishIngredient/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishIngredient = await _context.DishIngredient
                .Include(d => d.Dish)
                .Include(d => d.Ingredient)
                .FirstOrDefaultAsync(m => m.DishId == id);
            if (dishIngredient == null)
            {
                return NotFound();
            }

            return View(dishIngredient);
        }

        // POST: DishIngredient/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dishIngredient = await _context.DishIngredient.FindAsync(id);
            if (dishIngredient != null)
            {
                _context.DishIngredient.Remove(dishIngredient);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishIngredientExists(int id)
        {
            return _context.DishIngredient.Any(e => e.DishId == id);
        }
    }
}
