using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShoppingCart.Controllers;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Concrete;
using ShoppingCart.Models.Repositories.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ShoppingCart.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Can_Display_Product_And_Paginate()
        {
            // Arrange
            //Mock<IGenericRepository<Product>> mock = new Mock<IGenericRepository<Product>>();
            //mock.Setup(m => m.Add(new Product[] {
            //    new Product {Name = "P1", Description = "Desc product 1", Price = 5 },
            //    new Product {Name = "P2", Description = "Desc product 2", Price = 10 },
            //    new Product {Name = "P3", Description = "Desc product 3", Price = 15 },
            //    new Product {Name = "P4", Description = "Desc product 4", Price = 20 },
            //    new Product {Name = "P5", Description = "Desc product 5", Price = 25 }
            //}));
            //HomeController controller = new HomeController(mock.Object);
            //controller.PageSize = 3;

            //// Act
            //IEnumerable<Product> result = (IEnumerable<Product>)controller.Index(2).Model;

            //// Assert
            //Product[] prodArray = result.ToArray();
            //Assert.IsTrue(prodArray.Length == 2);
            //Assert.AreEqual(prodArray[0].Name, "P4");
            //Assert.AreEqual(prodArray[1].Name, "P5");

        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController(new GenericRepository<Product>());

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController(new GenericRepository<Product>());

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
