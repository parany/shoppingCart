using System;
using ShoppingCart.Models.Models.User;

namespace ShoppingCart.Models.Models.Entities
{
    public class ShippingDetail : BaseObject
    {
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public Guid CartId { get; set; }

        public virtual Cart Cart { get; set; }

    }
}
