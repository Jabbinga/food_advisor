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
    public class DishController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DishController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Dish
        public async Task<IActionResult> Index(string searchString, string sortOrder, int? tagId)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentTag"] = tagId;

            // Fetch dishes without sorting
            var dishes = from d in _context.Dish
                    .Include(d => d.DishTags)
                    .ThenInclude(dt => dt.Tag)
                    .Include(d => d.DishIngredients)
                    .ThenInclude(di => di.Ingredient)
                select d;

            if (!String.IsNullOrEmpty(searchString))
            {
                dishes = dishes.Where(d => d.Name.ToLower().Contains(searchString.ToLower()));
            }

            // Pass data for view
            ViewData["Tags"] = new SelectList(_context.Tag, "Id", "Name");
            ViewData["SelectedTagName"] = tagId.HasValue
                ? (await _context.Tag.FindAsync(tagId.Value))?.Name ?? "All Tags"
                : "All Tags";

            return View(await dishes.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Sort(string searchString, string sortOrder, int? tagId)
        {
            // Handle sorting logic here
            var dishes = from d in _context.Dish
                    .Include(d => d.DishTags)
                    .ThenInclude(dt => dt.Tag)
                    .Include(d => d.DishIngredients)
                    .ThenInclude(di => di.Ingredient)
                select d;

            if (!String.IsNullOrEmpty(searchString))
            {
                dishes = dishes.Where(d => d.Name.ToLower().Contains(searchString.ToLower()));
            }

            if (tagId.HasValue)
            {
                dishes = dishes.Where(d => d.DishTags.Any(dt => dt.TagId == tagId));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    dishes = dishes.OrderByDescending(d => d.Name);
                    break;
                case "Kcal":
                    dishes = dishes.OrderBy(d => d.Kcal);
                    break;
                case "kcal_desc":
                    dishes = dishes.OrderByDescending(d => d.Kcal);
                    break;
                default:
                    dishes = dishes.OrderBy(d => d.Name);
                    break;
            }

            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentTag"] = tagId;
            ViewData["NameSortParam"] = sortOrder == "name_desc" ? "name_desc" : "";
            ViewData["KcalSortParam"] = sortOrder == "Kcal" ? "Kcal" : "kcal_desc";

            ViewData["Tags"] = new SelectList(_context.Tag, "Id", "Name");

            var selectedTag = await _context.Tag.FindAsync(tagId.GetValueOrDefault());
            ViewData["SelectedTagName"] = selectedTag?.Name ?? "All Tags";

            return View("Index", await dishes.ToListAsync());
        }


        // GET: Dish/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish
                .Include(d => d.DishTags)
                .ThenInclude(dt => dt.Tag)
                .Include(d => d.DishIngredients)
                .ThenInclude(di => di.Ingredient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // GET: Dish/Create
        public IActionResult Create()
        {
            var viewModel = new DishCreateViewModel
            {
                Ingredients = _context.Ingredients.ToList(),
                Tags = _context.Tag.ToList()
            };
            return PartialView("_CreatePartial", viewModel);
        }

        // POST: Dish/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DishCreateViewModel viewModel, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                var dish = new Dish
                {
                    Name = viewModel.Name,
                    Kcal = viewModel.Kcal
                };

                if (imageFile != null)
                {
                    string uniqueFileName = Path.GetFileNameWithoutExtension(imageFile.FileName) + "_" +
                                            Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                    string filePath = Path.Combine("wwwroot/images", uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    dish.ImagePath = "/images/" + uniqueFileName;
                }

                _context.Add(dish);
                await _context.SaveChangesAsync();

                if (viewModel.SelectedIngredients != null && viewModel.SelectedIngredients.Any())
                {
                    foreach (var ingredientId in viewModel.SelectedIngredients)
                    {
                        _context.DishIngredient.Add(
                            new DishIngredient { DishId = dish.Id, IngredientId = ingredientId });
                    }

                    await _context.SaveChangesAsync();
                }

                if (viewModel.SelectedTags != null && viewModel.SelectedTags.Any())
                {
                    foreach (var tagId in viewModel.SelectedTags)
                    {
                        _context.DishTags.Add(new DishTag { DishId = dish.Id, TagId = tagId });
                    }

                    await _context.SaveChangesAsync();
                }

                return Json(new { success = true });
            }

            viewModel.Ingredients = _context.Ingredients.ToList();
            viewModel.Tags = _context.Tag.ToList();
            return PartialView("_CreatePartial", viewModel);
        }

        // GET: Dish/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }

            return PartialView("_EditPartial", dish); // Return partial view
        }

        // POST: Dish/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Kcal,ImagePath")] Dish dish, IFormFile imageFile)
        {
            if (id != dish.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null)
                    {
                        string uniqueFileName = Path.GetFileNameWithoutExtension(imageFile.FileName) + "_" +
                                                Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                        string filePath = Path.Combine("wwwroot/images", uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        dish.ImagePath = "/images/" + uniqueFileName;
                    }

                    _context.Update(dish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(dish.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return Json(new { success = true });
            }

            return PartialView("_EditPartial", dish);
        }

        // GET: Dish/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        [Authorize(Roles = "Administrator")]
        // POST: Dish/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await _context.Dish.FindAsync(id);
            if (dish != null)
            {
                _context.Dish.Remove(dish);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
            return _context.Dish.Any(e => e.Id == id);
        }
    }
}