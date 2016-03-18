using System;

namespace ShoppingCart.Models.Models.Entities
{

    public class ShippingDetail : BaseObject
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

    }
}
