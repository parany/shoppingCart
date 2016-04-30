using System.Collections.Generic;

namespace ShoppingCart.Models.Models.Entities
{
    public class Provider : BaseObject
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PaymentMethods { get; set; }
        public List<Product> Products { get; set; }
    }
}
