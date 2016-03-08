using ShoppingCart.Models.Models.User;

namespace ShoppingCart.Models.Models.Entities
{
    public class ShippingDetail : BaseObject
    {
        public ApplicationUser User { get; set; }

        public int UserId { get; set; }

        public Cart Cart { get; set; }

        public int CartId { get; set; }
    }
}
