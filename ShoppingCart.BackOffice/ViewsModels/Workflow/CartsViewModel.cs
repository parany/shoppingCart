using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Models.User;

namespace ShoppingCart.BackOffice.ViewsModels.Workflow
{
    public class CartsViewModel
    {
        public IList<CartWorkViewModel> Carts { get; set; }
        public class CartWorkViewModel
        {
            public Cart Cart { get; set; }
            public ApplicationUser User { get; set; }
            public string Status { get; set; }
        }
    }
}