﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.ViewModels
{
    public class CartLineViewModel
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}