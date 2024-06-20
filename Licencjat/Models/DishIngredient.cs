using System.ComponentModel.DataAnnotations;

namespace Licencjat.Models
{
    public class DishIngredient
    {
        [Key]
        public int DishId { get; set; }
        public Dish? Dish { get; set; }

        [Key]
        public int IngredientId { get; set; }
        public Ingredient? Ingredient { get; set; }
    }
}