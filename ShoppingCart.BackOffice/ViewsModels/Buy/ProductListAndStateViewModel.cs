using ShoppingCart.Models.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.BackOffice.ViewsModels.Buy
{
    public class ProductListAndStateViewModel
    {
        public IEnumerable<ProductStateViewModel> ProductAndState { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}