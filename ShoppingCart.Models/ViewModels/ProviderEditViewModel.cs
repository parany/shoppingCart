using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ShoppingCart.Models.ViewModels
{
    public class ProviderEditViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<SelectListItem> PaymentMethods { get; set; }
    }
}