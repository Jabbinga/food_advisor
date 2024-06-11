using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Licencjat.Models;

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

    // Navigation property for the join table
    public ICollection<DishTag> DishTags { get; set; } = new List<DishTag>();

    // TODO zamien jak skonfigurujesz identity 
    //public AppUser? User { get; set; }
    //[DisplayName("User")]
    //public string? UserId { get; set; }
}
