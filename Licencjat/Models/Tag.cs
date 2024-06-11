using System.ComponentModel.DataAnnotations;

namespace Licencjat.Models
{
    public class Tag
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string? Name { get; set; }

        // Navigation property for the join table
        public ICollection<DishTag> DishTags { get; set; } = new List<DishTag>();
    }
}
