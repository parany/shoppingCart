using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Models.Entities.Shopping
{
    public class Cart : BaseObject
    {
        public HashSet<CartLine> CartLines { get; set; }

        public Cart()
        {
            CartLines = new HashSet<CartLine>();
        }
    }
}
