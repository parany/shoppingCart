namespace ShoppingCart.Models.Entities.Shopping
{
    public class Product : BaseObject
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public Category Category { get; set; }

        public int CategoryId { get; set; }
    }
}
