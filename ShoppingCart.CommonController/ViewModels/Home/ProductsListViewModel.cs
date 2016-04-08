using System.Collections.Generic;
using ShoppingCart.Models.Models.Entities;

namespace ShoppingCart.CommonController.ViewModels.Home
{
    public class ProductsListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}