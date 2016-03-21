using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingCart.Models.Models.Entities;

namespace ShoppingCart.BackOffice.ViewsModels
{
    public class CartsViewModel
    {
        public string UserName { get; set; }
        public List<CartLineViewModel> CartLines { get; set; }
        public ShippingDetailViewModel ShippingDetail { get; set; }
    }
}