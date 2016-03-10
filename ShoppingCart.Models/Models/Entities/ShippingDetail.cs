using System;

namespace ShoppingCart.Models.Models.Entities
{
    public class ShippingDetail : BaseObject
    {
        public string UserId { get; set; }

        public Guid CartId { get; set; }

        public virtual Cart Cart { get; set; }

    }
}
