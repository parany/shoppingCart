﻿using System;
using System.Collections.Generic;
using ShoppingCart.Models.Models.Entities;

namespace ShoppingCart.BackOffice.ViewsModels
{
    public class CartsViewModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public List<CartLineViewModel> CartLines { get; set; }
        public ShippingDetailViewModel ShippingDetail { get; set; }
        public ShippingState ShippingState { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}