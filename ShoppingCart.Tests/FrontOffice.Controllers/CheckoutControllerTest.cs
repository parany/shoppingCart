using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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
