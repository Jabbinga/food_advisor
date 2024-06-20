using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Licencjat.Models;

namespace Licencjat.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Licencjat.Models.Ingredient> Ingredients { get; set; } = default!;

    public DbSet<Licencjat.Models.IngredientType> IngredientType { get; set; } = default!;
    public DbSet<Licencjat.Models.Dish> Dish { get; set; } = default!;
    public DbSet<Licencjat.Models.Tag> Tag { get; set; } = default!;

    public DbSet<Licencjat.Models.DishTag> DishTags { get; set; } = default!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure the many-to-many relationship
        modelBuilder.Entity<DishTag>()
            .HasKey(dt => new { dt.DishId, dt.TagId });

        modelBuilder.Entity<DishTag>()
            .HasOne(dt => dt.Dish)
            .WithMany(d => d.DishTags)
            .HasForeignKey(dt => dt.DishId);

        modelBuilder.Entity<DishTag>()
            .HasOne(dt => dt.Tag)
            .WithMany(t => t.DishTags)
            .HasForeignKey(dt => dt.TagId);
        
        modelBuilder.Entity<DishIngredient>()
            .HasKey(di => new { di.DishId, di.IngredientId });

        modelBuilder.Entity<DishIngredient>()
            .HasOne(di => di.Dish)
            .WithMany(d => d.DishIngredients)
            .HasForeignKey(di => di.DishId);

        modelBuilder.Entity<DishIngredient>()
            .HasOne(di => di.Ingredient)
            .WithMany(i => i.DishIngredients)
            .HasForeignKey(di => di.IngredientId);
        
    }

public DbSet<Licencjat.Models.Ingredient> Ingredient { get; set; } = default!;

public DbSet<Licencjat.Models.DishIngredient> DishIngredient { get; set; } = default!;
}