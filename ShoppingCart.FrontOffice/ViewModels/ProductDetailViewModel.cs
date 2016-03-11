

using System.Collections.Generic;
using ShoppingCart.Models.Models.Entities;

namespace ShoppingCart.ViewModels
{
    public class ProductDetailViewModel
    {
        // Selected list of products
        /*
         * Used to access a list of products depending of the request 
         */
        public IList<Product> Products { get; set; }

    }
}