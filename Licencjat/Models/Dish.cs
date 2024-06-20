using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Licencjat.Models
{
    public class Dish
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

        public string? ImagePath { get; set; }

        // Navigation properties for the join tables
        public ICollection<DishTag> DishTags { get; set; } = new List<DishTag>();
        public ICollection<DishIngredient> DishIngredients { get; set; } = new List<DishIngredient>();
    }
}