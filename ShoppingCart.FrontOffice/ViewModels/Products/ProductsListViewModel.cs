using System.Collections.Generic;
using ShoppingCart.Models.Models.Entities;

namespace ShoppingCart.ViewModels
{
    public class ProductsListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}