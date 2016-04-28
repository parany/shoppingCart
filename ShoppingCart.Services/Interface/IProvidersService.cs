﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Models.ViewModels;

namespace ShoppingCart.Services.Interface
{
    public interface IProvidersService
    {
        void AddProvider(ProviderViewModel providerViewModel);
        ProviderEditViewModel EditProvider(Guid? id);
        ProviderViewModel GetDetails(Guid? id);
        void UpdateProvider(ProviderViewModel providerViewModel);
    }
}
