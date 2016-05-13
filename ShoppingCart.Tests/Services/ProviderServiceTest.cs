using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShoppingCart.Models.Repositories.Interface;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.ViewModels;
using ShoppingCart.Services.Implementation;

namespace ShoppingCart.Tests.Services
{
    [TestClass]
    public class ProviderServiceTest
    {
        [TestMethod]
        public void AddProvider_CallsProviderRepositoryAdd_WhenCalledWithProviderViewModel()
        {
            var providerRepository = new Mock<IGenericRepository<Provider>>();
            var providerViewModel = new ProviderViewModel();
            providerViewModel.PaymentMethods = new String[0];
            providerRepository.Setup(mock => mock.Add(It.IsAny<Provider>()));
            var providerService = new ProvidersService(providerRepository.Object);
            providerService.AddProvider(providerViewModel);
            providerRepository.Verify(mock => mock.Add(It.IsAny<Provider>()));
        }
    }
}
