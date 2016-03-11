using ShoppingCart.Models.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.ViewModels
{
    public class CartCheckQuantityViewModel
    {
        public Product Product { get; set; }
        public string ReturnUrl { get; set; }
        public List<int> ToQuantityInStock { get; set; }
    }
}