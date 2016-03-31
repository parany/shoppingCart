using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.ViewModels
{
    public class CartIndexViewModel
    {
        public CartViewModel Cart { get; set; }
        public string ReturnUrl { get; set; }
        public string ErrorMessage { get; set; }
    }
}