using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingCart.Infrastructure.Abstract;
using Moq;
using ShoppingCart.ViewModels;
using ShoppingCart.Controllers;

namespace ShoppingCart.Tests.FrontOffice.Controllers
{
    [TestClass]
    public class CheckoutControllerTest
    {
        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            // Arrange - create a mock order processor
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            // Arrange - create an empty cart
            CartViewModel cart = new CartViewModel();

        }
    }
}
