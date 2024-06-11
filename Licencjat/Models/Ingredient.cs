using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Licencjat.Models;

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

    [Required]
    [DisplayName("Type")]
    public IngredientType? IngredientType { get; set; }
    }
