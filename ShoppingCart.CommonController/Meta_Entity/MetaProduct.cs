using System;
using ShoppingCart.Models.Models.Entities;

namespace ShoppingCart.CommonController.Meta_Entity
{
    public class MetaProduct
    {
        public Guid ID { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Price { get; set; }

        public int Quantity { get; set; }

        public Image Image { get; set; }

        public string Category { get; set; }
    }
}