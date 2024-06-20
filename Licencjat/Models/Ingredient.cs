using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Licencjat.Models
{
    public class Ingredient
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string? Name { get; set; }

        [Required]
        [DisplayName("Kcal per 100g")]
        public uint Kcal { get; set; }

        // Navigation property for the join table
        public ICollection<DishIngredient> DishIngredients { get; set; } = new List<DishIngredient>();
    }
}