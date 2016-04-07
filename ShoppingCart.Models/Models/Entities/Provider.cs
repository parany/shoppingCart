using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Models.Models.Payments;

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
