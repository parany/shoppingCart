namespace ShoppingCart.Models.Models.Payments
{
    public class Check : Payment
    {
        public string OrderIdentity { get; set; }
        public string Address { get; set; }
    }
}
