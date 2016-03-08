namespace ShoppingCart.Models.Models.Entities
{
    public class CartLine : BaseObject
    {
        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }

        public Cart Cart { get; set; }

        public int CartId { get; set; }
    }
}
