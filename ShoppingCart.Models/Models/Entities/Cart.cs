using System;
using System.Collections.Generic;
using ShoppingCart.Models.Models.User;

namespace ShoppingCart.Models.Models.Entities
{
    public enum ShippingState
    {
        Pending = 0,
        Canceled = 1,
        Delivered = 2
    };
    public sealed class Cart : BaseObject
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public HashSet<CartLine> CartLines { get; set; }

        public Guid ShippingDetailId { get; set; }

        public ShippingDetail ShippingDetail { get; set; }

        public ShippingState State { get; set; }

        public string PaymentMethod { get; set; }

        public Cart()
        {
            CartLines = new HashSet<CartLine>();
        }
    }
}
