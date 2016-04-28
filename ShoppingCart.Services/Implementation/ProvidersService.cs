using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ShoppingCart.Models.ViewModels;
using ShoppingCart.Models.Repositories.Interface;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Models.Payments;
using ShoppingCart.Services.Interface;
using ShoppingCart.GeneralLib.Util;

namespace ShoppingCart.Services.Implementation
{
    public class ProvidersService : IProvidersService
    {
        private IGenericRepository<Provider> ProviderRepository { get; set; }
        public ProvidersService(IGenericRepository<Provider> providerRepository)
        {
            ProviderRepository = providerRepository;
        }
        private List<Payment> GetPaymentMethods()
        {
            var payments = new Payments();
            payments.InitPaymentsListFromResourceString(ResourcesHelper.payment);
            return payments.Modules;
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
        public ProviderEditViewModel EditProvider(Guid? id)
        {
            Provider provider = ProviderRepository.GetSingle(x => x.Id == id);
            ProviderEditViewModel providerViewModel = new ProviderEditViewModel();
            providerViewModel.Id = provider.Id;
            providerViewModel.Address = provider.Address;
            providerViewModel.Name = provider.Name;
            var paymentMethodsAll = GetPaymentMethods();
            string[] selectedPaymentMethods = null;
            if (provider.PaymentMethods != null)
                selectedPaymentMethods = provider.PaymentMethods.Split(',');
            var paymentMethods = new List<SelectListItem>();
            foreach (var pm in paymentMethodsAll)
            {
                var item = new SelectListItem
                {
                    Value = pm.Id.ToString(),
                    Text = pm.Name
                };
                paymentMethods.Add(item);
            }
            paymentMethods.ForEach(p => p.Selected = selectedPaymentMethods != null && selectedPaymentMethods.Contains(p.Value));
            providerViewModel.PaymentMethods = paymentMethods;
            return providerViewModel;
        }
        public ProviderViewModel GetDetails(Guid? id)
        {
            Provider provider = ProviderRepository.GetSingle(x => x.Id == id);
            if (provider == null)
                return null;
            var providerViewModel = new ProviderViewModel();
            providerViewModel.Id = provider.Id;
            providerViewModel.Address = provider.Address;
            providerViewModel.Name = provider.Name;
            if (provider.PaymentMethods != null)
                providerViewModel.PaymentMethods = provider.PaymentMethods.Split(',');
            return providerViewModel;
        }
        public void UpdateProvider(ProviderViewModel providerViewModel)
        {
            Provider provider = new Provider();
            var sbPaymentMethods = new StringBuilder();
            int i = 1;
            if (providerViewModel.PaymentMethods != null)
            {
                foreach (var p in providerViewModel.PaymentMethods)
                {
                    sbPaymentMethods.Append(p);
                    if (i < providerViewModel.PaymentMethods.Count())
                    {
                        sbPaymentMethods.Append(',');
                    }
                    i++;
                }
            }

            provider.Id = providerViewModel.Id;
            provider.Address = providerViewModel.Address;
            provider.Name = providerViewModel.Name;
            provider.PaymentMethods = sbPaymentMethods.ToString();

            ProviderRepository.Update(provider);
        }
    }
}
