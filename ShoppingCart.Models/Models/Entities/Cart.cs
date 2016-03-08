using System.Collections.Generic;

namespace ShoppingCart.Models.Models.Entities
{
    public sealed class Cart : BaseObject
    {
        public HashSet<CartLine> CartLines { get; set; }

        public Cart()
        {
            CartLines = new HashSet<CartLine>();
        }
    }
}
