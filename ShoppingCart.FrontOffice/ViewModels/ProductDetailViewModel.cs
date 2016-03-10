

using System.Collections.Generic;
using ShoppingCart.Models.Models.Entities;

namespace ShoppingCart.ViewModels
{
    public class ProductDetailViewModel
    {
        public IList<Product> Products { get; set; }
    }
}