using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Models.ViewModels;
using ShoppingCart.Models.Repositories.Interface;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Services.Interface;

namespace ShoppingCart.Services.Implementation
{
    public class ProvidersService : IProvidersService
    {
        private IGenericRepository<Provider> ProviderRepository { get; set; }
        public ProvidersService(IGenericRepository<Provider> providerRepository)
        {
            ProviderRepository = providerRepository;
        }
        public void AddProvider(ProviderViewModel providerViewModel)
        {
            Provider provider = new Provider();
            var sbPaymentMethods = new StringBuilder();
            int i = 1;
            foreach (var p in providerViewModel.PaymentMethods)
            {
                sbPaymentMethods.Append(p);
                if (i < providerViewModel.PaymentMethods.Count())
                {
                    sbPaymentMethods.Append(',');
                }
                i++;
            }

            provider.Address = providerViewModel.Address;
            provider.Name = providerViewModel.Name;
            provider.PaymentMethods = sbPaymentMethods.ToString();

            ProviderRepository.Add(provider);
        }
    }
}
