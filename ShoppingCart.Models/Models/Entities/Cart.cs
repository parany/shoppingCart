using System;
using System.Collections.Generic;
using ShoppingCart.Models.Models.User;

namespace ShoppingCart.Models.Models.Entities
{
    public enum ShippingState
    {
        Created = 0,
        Pending = 1,
        Canceled = 2,
        Delivered = 3,
        NotCheckedOut = 4
    };
    public sealed class Cart : BaseObject
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public HashSet<CartLine> CartLines { get; set; }

        public Guid ShoppingDetailId { get; set; }

        public ShippingDetail ShippingDetail { get; set; }

        public ShippingState State { get; set; }

        public Cart()
        {
            CartLines = new HashSet<CartLine>();
        }
    }
}
