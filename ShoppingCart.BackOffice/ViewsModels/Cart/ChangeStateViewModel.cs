using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingCart.Models.Models.Entities;

namespace ShoppingCart.BackOffice.ViewsModels
{
    public class ChangeStateViewModel
    {
        public Guid Id { get; set; }
        public ShippingState State { get; set; }
    }
}
