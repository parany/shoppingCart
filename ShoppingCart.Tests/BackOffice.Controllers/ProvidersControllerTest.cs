using Moq;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingCart.BackOffice.Controllers;
using ShoppingCart.Models.Repositories.Interface;
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
        public void Index_ReturnsViewResult_WhenCalled()
        {
            var mockServiceProvider = new Mock<IProvidersService>();
            var providerList = new List<Provider>();
            mockServiceProvider.Setup(serviceProvider => serviceProvider.GetAllProviders()).Returns(providerList);
            var providerController = new ProvidersController(mockServiceProvider.Object);
            var result = providerController.Index();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(((ViewResult)result).Model, providerList);
        }
    }
}
