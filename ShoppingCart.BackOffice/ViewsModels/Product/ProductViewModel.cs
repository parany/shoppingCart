﻿using ShoppingCart.Models.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.BackOffice.ViewsModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }

        public string ImagePath { get; set; }
    }
}