using System;

namespace ShoppingCart.Models.Models.Entities
{
    public class CartLine : BaseObject
    {
        public Guid ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int Quantity { get; set; }

        public Guid CartId { get; set; }

        public virtual Cart Cart { get; set; }

    }
}
