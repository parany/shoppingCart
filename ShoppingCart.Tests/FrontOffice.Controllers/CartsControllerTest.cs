using Moq;
using System;
using System.Linq;
using System.Web.Mvc;
using ShoppingCart.Models.Models.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ShoppingCart.Models.Repositories.Interface;
using ShoppingCart.Controllers;
using ShoppingCart.CommonController.ViewModels;

namespace ShoppingCart.Tests.FrontOffice.Controllers
{
    [TestClass]
    public class CartsControllerTest
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            // Arrange - create some test products
            Product p1 = new Product { Id = Guid.NewGuid(), Name = "P1" };
            Product p2 = new Product { Id = Guid.NewGuid(), Name = "P2" };
            // Arrange - create a new cart
            CartViewModel target = new CartViewModel();

            // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            CartLineViewModel[] results = target.Lines.ToArray();

            // Assert
            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Product, p1);
            Assert.AreEqual(results[1].Product, p2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            // Arrange - create some test products
            Product p1 = new Product { Id = Guid.NewGuid(), Name = "P1" };
            Product p2 = new Product { Id = Guid.NewGuid(), Name = "P2" };
            // Arrange - create a new cart
            CartViewModel target = new CartViewModel();

            // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 2);
            target.AddItem(p1, 5);
            CartLineViewModel[] results = target.Lines.OrderBy(c => c.Product.Name).ToArray();

            // Assert
            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Quantity, 6);
            Assert.AreEqual(results[1].Quantity, 2);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            // Arrange - create some test products
            Product p1 = new Product { Id = Guid.NewGuid(), Name = "P1" };
            Product p2 = new Product { Id = Guid.NewGuid(), Name = "P2" };
            Product p3 = new Product { Id = Guid.NewGuid(), Name = "P3" };
            // Arrange - create a new cart
            CartViewModel target = new CartViewModel();
            // Arrange - add some products to the cart
            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3, 5);
            target.AddItem(p2, 1);

            // Act
            target.RemoveLine(p2);

            // Assert
            Assert.AreEqual(target.Lines.Where(c => c.Product == p2).Count(), 0);
            Assert.AreEqual(target.Lines.Count(), 2);
        }

        [TestMethod]
        public void Can_Calculate_Cart_Total()
        {
            // Arrange - create some test products
            Product p1 = new Product { Id = Guid.NewGuid(), Name = "P1", Price = 100M };
            Product p2 = new Product { Id = Guid.NewGuid(), Name = "P2", Price = 50M };
            // Arrange - create a new cart
            CartViewModel target = new CartViewModel();

            // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);
            decimal result = target.ComputeTotalValue();

            // Assert
            Assert.AreEqual(result, 450M);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            // Arrange - create some test products
            Product p1 = new Product { Id = Guid.NewGuid(), Name = "P1", Price = 100M };
            Product p2 = new Product { Id = Guid.NewGuid(), Name = "P2", Price = 50M };
            // Arrange - create a new cart
            CartViewModel target = new CartViewModel();
            // Arrange - add some items
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);

            // Act - reset the cart
            target.Clear();

            // Assert
            Assert.AreEqual(target.Lines.Count(), 0);
        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            // Arrange - create the mock repository
            Mock<IGenericRepository<Product>> mockProduct = new Mock<IGenericRepository<Product>>();
            Mock<IGenericRepository<Category>> mockCategory = new Mock<IGenericRepository<Category>>();
            Mock<IGenericRepository<Cart>> mockCart = new Mock<IGenericRepository<Cart>>();
            Mock<IGenericRepository<ShippingDetail>> mockShippingDetail = new Mock<IGenericRepository<ShippingDetail>>();
            Mock<IGenericRepository<CartLine>> mockCartLine = new Mock<IGenericRepository<CartLine>>();

            Category c1 = new Category { Id = Guid.NewGuid(), Name = "C1" };
            mockCategory.Setup(m => m.Add(c1));
            Product p1 = new Product { Id = Guid.NewGuid(), Name = "P1", CategoryId = c1.Id, Price = 150 };
            Product p2 = new Product { Id = Guid.NewGuid(), Name = "P2", CategoryId = c1.Id, Price = 200 };
            Product[] list = new Product[] {p1,p2};
            mockProduct.Setup(m => m.Add(list));

            // Arrange - create a Cart
            CartViewModel cart = new CartViewModel();
            CartCheckQuantityViewModel chkQtty = new CartCheckQuantityViewModel
            {
                Product = p1,
                QtyOrdered = 2,
                QtyRemaining = 4,
                ReturnUrl = "/"
            };
            // Arrange - create the controller
            CartsController target = new CartsController(mockProduct.Object, mockCart.Object, mockShippingDetail.Object, mockCartLine.Object);
            
            // Act - add a product to the cart
            target.AddToCart(chkQtty, cart);
            
            // Assert
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.Id, p1.Id);
        }

        [TestMethod]
        public void Adding_Product_To_Cart_Goes_To_Cart_Screen()
        {
            // Arrange - create the mock repository
            Mock<IGenericRepository<Product>> mockProduct = new Mock<IGenericRepository<Product>>();
            Mock<IGenericRepository<Category>> mockCategory = new Mock<IGenericRepository<Category>>();
            Mock<IGenericRepository<Cart>> mockCart = new Mock<IGenericRepository<Cart>>();
            Mock<IGenericRepository<ShippingDetail>> mockShippingDetail = new Mock<IGenericRepository<ShippingDetail>>();
            Mock<IGenericRepository<CartLine>> mockCartLine = new Mock<IGenericRepository<CartLine>>();

            Category c1 = new Category { Id = Guid.NewGuid(), Name = "C1" };
            mockCategory.Setup(m => m.Add(c1));
            Product p1 = new Product { Id = Guid.NewGuid(), Name = "P1", CategoryId = c1.Id, Price = 150 };
            Product p2 = new Product { Id = Guid.NewGuid(), Name = "P2", CategoryId = c1.Id, Price = 200 };
            Product[] list = new Product[] { p1, p2 };
            mockProduct.Setup(m => m.Add(list));

            // Arrange - create a Cart
            CartViewModel cart = new CartViewModel();
            // Arrange - create the controller
            CartsController target = new CartsController(mockProduct.Object, mockCart.Object, mockShippingDetail.Object, mockCartLine.Object);
            CartCheckQuantityViewModel chkQtty = new CartCheckQuantityViewModel
            {
                Product = p1,
                QtyOrdered = 2,
                QtyRemaining = 4,
                ReturnUrl = "/"
            };
            // Act
            RedirectToRouteResult result = target.AddToCart(chkQtty, cart);

            // Assert
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "/");
        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            // Arrange - create the mock repository
            Mock<IGenericRepository<Product>> mockProduct = new Mock<IGenericRepository<Product>>();
            Mock<IGenericRepository<Category>> mockCategory = new Mock<IGenericRepository<Category>>();
            Mock<IGenericRepository<Cart>> mockCart = new Mock<IGenericRepository<Cart>>();
            Mock<IGenericRepository<ShippingDetail>> mockShippingDetail = new Mock<IGenericRepository<ShippingDetail>>();
            Mock<IGenericRepository<CartLine>> mockCartLine = new Mock<IGenericRepository<CartLine>>();

            // Arrange - create a Cart
            CartViewModel cart = new CartViewModel();
            // Arrange - create the controller
            CartsController target = new CartsController(mockProduct.Object, mockCart.Object, mockShippingDetail.Object, mockCartLine.Object);

            // Act - call the Index action method
            CartIndexViewModel result = (CartIndexViewModel)target.Index(cart, "testUrl").ViewData.Model;
            // Assert
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "testUrl");
        }
    }
}
