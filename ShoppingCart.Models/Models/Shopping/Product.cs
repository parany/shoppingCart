namespace ShoppingCart.Models.Models.Shopping
{
    public class Product : BaseObject
    {
        public string Name { get; set; }

        public Category Category { get; set; }

        public int CategoryId { get; set; }
    }
}
