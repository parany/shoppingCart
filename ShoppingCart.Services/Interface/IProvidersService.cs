using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.ViewModels;

namespace ShoppingCart.Services.Interface
{
    public interface IProvidersService
    {
        void AddProvider(ProviderViewModel providerViewModel);
        ProviderEditViewModel EditProvider(Guid? id);
        ProviderViewModel GetDetails(Guid? id);
        void UpdateProvider(ProviderViewModel providerViewModel);
        Provider GetProvider(Guid? id);
        void DeleteProvider(Guid id);
        IEnumerable<Provider> GetAllProviders();
    }
}
