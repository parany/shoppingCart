using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.BackOffice.ViewsModels.Provider
{
    public class ProviderViewModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string[] PaymentMethods { get; set; }
    }
}