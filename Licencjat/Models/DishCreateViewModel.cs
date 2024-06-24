namespace Licencjat.Models;

public class DishCreateViewModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public uint Kcal { get; set; }
    public string? ImagePath { get; set; }
    public List<int> SelectedIngredients { get; set; } = new List<int>();
    public IEnumerable<Ingredient>? Ingredients { get; set; }
}