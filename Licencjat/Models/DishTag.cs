namespace Licencjat.Models
{
    public class DishTag
    {
        public int DishId { get; set; }
        public Dish? Dish { get; set; }

        public int TagId { get; set; }
        public Tag? Tag { get; set; }
    }
}
