namespace ShoppingCart.Models.Models.Entities
{
    public class CartLine : BaseObject
    {
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int Quantity { get; set; }

        public int CartId { get; set; }

        public virtual Cart Cart { get; set; }

    }
}
