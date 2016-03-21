using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;
using ShoppingCart.ViewModels;
using Microsoft.AspNet.Identity;

namespace ShoppingCart.Controllers
{
    public class CartsController : Controller
    {
        private IGenericRepository<Product> _ProductRepository { get; set; }
        private IGenericRepository<Cart> _CartRepository { get; set; }
        private IGenericRepository<ShippingDetail> _ShipRepository { get; set; }
        private IGenericRepository<CartLine> _CartLineRepository { get; set; }

        public CartsController(IGenericRepository<Product> productRepository, 
                               IGenericRepository<Cart> cartRepository, 
                               IGenericRepository<ShippingDetail> shipRepository,
                               IGenericRepository<CartLine> cartlineRepository)
        {
            _ProductRepository = productRepository;
            _CartRepository = cartRepository;
            _CartRepository.AddNavigationProperty(x => x.CartLines);
            _CartLineRepository = cartlineRepository;
            _CartLineRepository.AddNavigationProperty(x=> x.Product);
            _ShipRepository = shipRepository;
        }
        // GET: /Carts/Index
        // Action Showing the content of the cart
        public ViewResult Index(CartViewModel cart, string returnUrl, string errorMessage = "")
        {
            // Passing view model containing the cart, the return url and error message
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl ?? "/",
                ErrorMessage = errorMessage
            });
        }

        // GET: /Carts/CheckEmpty
        // Action to Verify if carts is empty
        public RedirectToRouteResult CheckEmpty(CartViewModel cart)
        {
            if (cart.Lines.Count() == 0)
            {
                return RedirectToAction("Index", "Carts", 
                    new { errorMessage = "Empty Cart! Please fill the cart before checking out." });
            }
            else
            {
                return RedirectToAction("Index","Checkout");
            }
        }

        // POST: /Carts/checkQuantity
        // Action permitting the customer to enter quantity of product to add to cart
        [HttpPost]
        public ViewResult checkQuantity(Guid productId, string returnUrl, CartViewModel cart)
        {

            // Getting the product to add to cart and the quantity already in cart
            Product product = _ProductRepository.GetSingle(p => p.Id == productId);
            int Qty = cart.GetQuantity(product);
            int qtyOrdered = 1;

            // View model containing
            // * product
            // * return Url
            // * default product's quantity to add to cart (1)
            // * the quantity availaible to add to cart( In Stock - Already in Cart )
            CartCheckQuantityViewModel chkQtty = new CartCheckQuantityViewModel
            {
                Product = product,
                ReturnUrl = returnUrl,
                QtyOrdered = qtyOrdered,
                QtyRemaining = product.Quantity - Qty
            };
            return View("AddToCart", chkQtty);
        }

        // POST: /Carts/AddToCart
        // Action permitting to add to cart after having the quantity to add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public RedirectToRouteResult AddToCart(CartCheckQuantityViewModel data, CartViewModel cart)
        {
            // Retrieving product to add
            Product product = _ProductRepository.GetSingle(p => p.Id == data.Product.Id);
            // Adding to cart
            if (product != null)
            {
                cart.AddItem(product, data.QtyOrdered);
            }
            // Redirecting to populate the new content of the cart
            return RedirectToAction("Index", new { data.ReturnUrl });
        }

        // POST: /Carts/RemoveFromCart
        // Action permitting to remove a product from the cart
        [HttpPost]
        public RedirectToRouteResult RemoveFromCart(CartViewModel cart, Guid productId, string returnUrl)
        {
            // Retrieving product to remove
            Product product = _ProductRepository.GetSingle(p => p.Id == productId);
            // Removing from cart
            if (product != null)
            {
                cart.RemoveLine(product);
            }
            // Redirecting to populate the remaining content of the cart
            return RedirectToAction("Index", new { returnUrl });
        }

        // GET: /Carts/Summary
        // Action for populating the summary of the content of the cart
        public PartialViewResult Summary(CartViewModel cart)
        {
            return PartialView(cart);
        }

    }
}