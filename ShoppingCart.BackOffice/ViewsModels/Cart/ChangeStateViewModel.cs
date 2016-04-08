using System;
using ShoppingCart.Models.Models.Entities;

namespace ShoppingCart.BackOffice.ViewsModels
{
    public class ChangeStateViewModel
    {
        public Guid Id { get; set; }
        public ShippingState State { get; set; }
    }
}
