namespace ShoppingCart.Models.Models.Entities
{
    public class ShippingDetail : BaseObject
    {
        public string UserId { get; set; }

        public int CartId { get; set; }

        public virtual Cart Cart { get; set; }

    }
}
