using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingCart.Models.Models.User;

namespace ShoppingCart.ViewModels
{
    public class CheckoutViewModel
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public CartViewModel CartViewmodel { get; set; }
    }
}