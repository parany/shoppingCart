using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingCart.BackOffice.Controllers;
using ShoppingCart.Models.ViewModels;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Services.Interface;
using System.Web.Mvc;
using System.Net;

namespace ShoppingCart.Tests.BackOffice.Controllers
{
    [TestClass]
    public class ProvidersControllerTest
    {
        [TestMethod]
        public void Edit_ReturnsHttpStatusBadRequest_WhenIdIsNull()
        {
            var mockServiceProvider = new Mock<IProvidersService>();
            var providerController = new ProvidersController(mockServiceProvider.Object);
            Guid? nullId = null;
            var result = providerController.Edit(nullId);
            Assert.IsInstanceOfType(result, typeof(HttpStatusCodeResult));
            Assert.AreEqual(new HttpStatusCodeResult(HttpStatusCode.BadRequest).StatusCode,
                            ((HttpStatusCodeResult)result).StatusCode);
        }
        [TestMethod]
        public void Index_ReturnsViewResult_WhenCalledWithoutParameters()
        {
            var mockServiceProvider = new Mock<IProvidersService>();
            var providerList = new List<Provider>();
            mockServiceProvider.Setup(serviceProvider => serviceProvider.GetAllProviders()).Returns(providerList);
            var providerController = new ProvidersController(mockServiceProvider.Object);
            var result = providerController.Index();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(((ViewResult)result).Model, providerList);
        }
        [TestMethod]
        public void Index_HasAuthorizeAttribute()
        {
            var mockServiceProvider = new Mock<IProvidersService>();
            var providerController = new ProvidersController(mockServiceProvider.Object);
            var type = providerController.GetType();
            var methodInfo = type.GetMethod("Index");
            var attributes = methodInfo.GetCustomAttributes(typeof(AuthorizeAttribute), true);
            Assert.IsTrue(attributes.Any());
        }
        [TestMethod]
        public void Details_ReturnsHttpStatusBadRequest_WhenIdIsNull()
        {
            var mockServiceProvider = new Mock<IProvidersService>();
            var providerController = new ProvidersController(mockServiceProvider.Object);
            Guid? nullId = null;
            var result = providerController.Details(nullId);
            Assert.IsInstanceOfType(result, typeof(HttpStatusCodeResult));
            Assert.AreEqual(new HttpStatusCodeResult(HttpStatusCode.BadRequest).StatusCode,
                            ((HttpStatusCodeResult)result).StatusCode);
        }
        [TestMethod]
        public void Details_HasAuthorizeAttribute()
        {
            var mockServiceProvider = new Mock<IProvidersService>();
            var providerController = new ProvidersController(mockServiceProvider.Object);
            var type = providerController.GetType();
            var methodInfo = type.GetMethod("Details");
            var attributes = methodInfo.GetCustomAttributes(typeof(AuthorizeAttribute), true);
            Assert.IsTrue(attributes.Any());
        }
        [TestMethod]
        public void Details_ReturnsHttpNotFound_WhenIdNotFound()
        {
            var mockServiceProvider = new Mock<IProvidersService>();
            ProviderViewModel nullProvider = null;
            mockServiceProvider.Setup(serviceProvider => serviceProvider.GetDetails(It.IsAny<Guid?>())).Returns(nullProvider);
            var providersController = new ProvidersController(mockServiceProvider.Object);
            var result = providersController.Details(It.IsAny<Guid>());
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }
        [TestMethod]
        public void Details_ReturnsViewResult_WhenCalledWithId()
        {
            var mockServiceProvider = new Mock<IProvidersService>();
            ProviderViewModel providerViewModel = new ProviderViewModel();
            mockServiceProvider.Setup(serviceProvider => serviceProvider.GetDetails(It.IsAny<Guid?>())).Returns(providerViewModel);
            var providersController = new ProvidersController(mockServiceProvider.Object);
            var result = providersController.Details(It.IsAny<Guid>());
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(((ViewResult)result).Model, providerViewModel);
        }
    }
}
