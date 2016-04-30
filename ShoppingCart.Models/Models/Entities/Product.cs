using ShoppingCart.Models.Log;
using System;
using System.Collections.Generic;

namespace ShoppingCart.Models.Models.Entities
{
    public enum ProductType
    {
        ForSale = 0,
        ToBuy = 1
    };

    [LoggingClass]
    public class Product : BaseObject
    {
        public string Name { get; set; }

        public string Description { get; set; }

        [Logging]
        public decimal Price { get; set; }

        [Logging]
        public int Quantity { get; set; }

        public Guid ImageId { get; set; }

        public virtual Image Image { get; set; }

        public Guid CategoryId { get; set; }

        public Guid ProductReference { get; set; }

        public ProductType Type { get; set; }
    
        public virtual Category Category { get; set; }

        public List<Provider> Providers { get; set; }

    }
}
