using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;
using ShoppingCart.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.Controllers
{
    public class CartsController : Controller
    {
        private IGenericRepository<Product> ProductRepository { get; set; }
        private IGenericRepository<ShippingDetail> ShippingDetails { get; set; } 
        public int PageSize = 3;

        public CartsController(IGenericRepository<Product> productRepository)
        {
            ProductRepository = productRepository;
        }

        public ViewResult Index(CartViewModel cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public ViewResult checkQuantity(Guid productId, string returnUrl)
        {
            Product product = ProductRepository.GetSingle(p => p.Id == productId);
            List<int> toQuantityInStock = new List<int>();
            for(int i=0; i < product.Quantity; i++)
            {
                toQuantityInStock.Add(i + 1);
            }
            CartCheckQuantityViewModel chkQtty = new CartCheckQuantityViewModel
            {
                Product = product,
                ReturnUrl = returnUrl,
                ToQuantityInStock = toQuantityInStock
            };
            return View("AddToCart", chkQtty);
        }

        public RedirectToRouteResult AddToCart(CartViewModel cart, Guid productId, string returnUrl)
        {
            Product product = ProductRepository.GetSingle(p => p.Id == productId);
            if (product != null)
            {
                cart.AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(CartViewModel cart, Guid productId, string returnUrl)
        {
            Product product = ProductRepository.GetSingle(p => p.Id == productId);
            if (product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public PartialViewResult Summary(CartViewModel cart)
        {
            return PartialView(cart);
        }

        [Authorize]
        public ViewResult Checkout()
        {
            return View();
        }

    }
}