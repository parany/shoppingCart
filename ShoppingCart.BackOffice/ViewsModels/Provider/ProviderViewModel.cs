using System;

namespace ShoppingCart.BackOffice.ViewsModels.Provider
{
    public class ProviderViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string[] PaymentMethods { get; set; }
    }
}