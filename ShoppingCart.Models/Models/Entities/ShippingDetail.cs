using System;
using ShoppingCart.Models.Models.User;

namespace ShoppingCart.Models.Models.Entities
{
    public enum ShippingState
    {
        NotCheckedOut = 0,
        Pending = 1,
        Canceled = 2,
        Delivered = 3
    };

    public class ShippingDetail : BaseObject
    {
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public Guid CartId { get; set; }

        public virtual Cart Cart { get; set; }

        public ShippingState State { get; set; }
    }
}
