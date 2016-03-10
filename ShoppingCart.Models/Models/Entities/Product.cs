using System;

namespace ShoppingCart.Models.Models.Entities
{
    public class Product : BaseObject
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public Guid CategoryId { get; set; }

        public virtual Category Category { get; set; }

    }
}
