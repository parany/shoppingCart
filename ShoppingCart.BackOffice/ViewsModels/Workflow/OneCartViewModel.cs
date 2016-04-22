using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingCart.CommonController.Tools;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Models.User;

namespace ShoppingCart.BackOffice.ViewsModels.Workflow
{
    public class OneCartViewModel
    {
        public Cart Cart { get; set; }

        public ApplicationUser User { get; set; }
        public IList<CartLineViewModel> CartLines { get; set; } 
        public IList<CartProcessTree.NodeObject> Options { get; set; }
        public IList<CartProcessTree.NodeObject> Forms { get; set; }

        public string status { get; set; }
        public IList<string> Evolution { get; set; }

        public class CartLineViewModel
        {
            public Product Product { get; set; }
            public int Quantity { get; set; }
            public decimal Total { get; set; }
        }
    }
}