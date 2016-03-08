using System.Collections.Generic;

namespace ShoppingCart.Models.Entities.Shopping
{
    public class Category : BaseObject
    {
        public string Name { get; set; }

        public HashSet<Product> Products { get; set; }

        public Category()
        {
            Products = new HashSet<Product>();
        }
    }
}
