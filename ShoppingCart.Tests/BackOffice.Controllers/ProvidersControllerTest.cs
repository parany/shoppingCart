using Moq;
using System;
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
    }
}
