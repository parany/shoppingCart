using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingCart.Models.Models.Entities;

namespace ShoppingCart.ViewModels
{
    public class CartDTO
    {
        public Cart Cart { get; set; }
        public string UserId { get; set; }

        public string UserName { get; set; }
    }
}