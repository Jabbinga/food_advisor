using System.ComponentModel.DataAnnotations;

namespace Licencjat.Models;

public class IngredientType
{
    [Required]
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string? Name { get; set; }
}