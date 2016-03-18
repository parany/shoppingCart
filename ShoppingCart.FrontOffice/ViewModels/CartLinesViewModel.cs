using ShoppingCart.Models.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.ViewModels
{
    public class CartLinesViewModel
    {
        public IEnumerable<CartLine>  CartLines { get; set; }
    }
}