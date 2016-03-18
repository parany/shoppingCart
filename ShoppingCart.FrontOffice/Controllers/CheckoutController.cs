using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

using ShoppingCart.ViewModels;
using ShoppingCart.Models.Repositories.Interface;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Infrastructure.Binders;
using ShoppingCart.Infrastructure.Abstract;

namespace ShoppingCart.Controllers
{
    
    public class CheckoutController : Controller
    {
        private IOrderProcessor _OrderProcessor;
        private IGenericRepository<Cart> _CartRepository { get; set; }
        private IGenericRepository<Product> _ProductRepository { get; set; }
        private IGenericRepository<CartLine> _CartLineRepository { get; set; }
        private IGenericRepository<ShippingDetail> _ShippingRepository { get; set; }

        public CheckoutController(IGenericRepository<Cart> cartRepo,
                                  IGenericRepository<Product> productRepo,
                                  IGenericRepository<CartLine> cartlineRepo,
                                  IGenericRepository<ShippingDetail> shipRepo,
                                  IOrderProcessor proc)
        {
            _ProductRepository = productRepo;
            _CartRepository = cartRepo;
            _CartRepository.AddNavigationProperties(ca => ca.CartLines);
            _CartLineRepository = cartlineRepo;
            _CartLineRepository.AddNavigationProperty(cl => cl.Product);
            _CartLineRepository.AddNavigationProperty(clr => clr.Cart);
            _ShippingRepository = shipRepo;
            _OrderProcessor = proc;
        }

        // GET: /Checkout/Index or /Checkout
        // Action for populating a summary of the order and associating the order to a customer
        [Authorize]
        public ActionResult Index(CartViewModel cartView, String errorMessage = "")
        {

            // Verifying that cart are not empty before checking out
            if (cartView.Lines.Count() == 0)
            {
                return RedirectToAction("Index", "Carts", new { errorMessage = "Empty Cart! Please fill the cart before checking out." });
            }
            else
            { 
                // Creating ViewModel to pass to the view for rendering summary
                CartDTO cdto = new CartDTO
                {
                    Cart = cartView,
                    UserName = User.Identity.GetUserName(),
                    ErrorMessage = errorMessage
                };
                return View(cdto);
            }
        }

        // POST: /Checkout/Order
        // Action confirming the order
        [Authorize]
        public ActionResult Order(CartDTO cartDto)
        {
            // Creating a new cart in persistence
            Cart cart = new Cart
            {
                Id = Guid.NewGuid(),
                DateCreated = DateTime.Now,
                CreatedBy = User.Identity.GetUserId()
            };
            _CartRepository.Add(cart);

            // Creating new cartline for each line in the cart and adding to cart in persistence
            foreach (CartLineViewModel cartl in cartDto.Cart.Lines)
            {
                CartLine cartline = new CartLine
                {
                    Id = Guid.NewGuid(),
                    CartId = cart.Id,
                    ProductId = cartl.Product.Id,
                    Quantity = cartl.Quantity,
                    DateCreated = DateTime.Now,
                    CreatedBy = User.Identity.GetUserId()
                };
                _CartLineRepository.Add(cartline);

                cart.CartLines.Add(cartline);
            }
            cart.DateModified = DateTime.Now;
            cart.ModifiedBy = User.Identity.GetUserId();
            _CartRepository.Update(cart);

            // Creating Shipping Details for associating a cart with a user
            ShippingDetail shipD = new ShippingDetail
            {
                Id = Guid.NewGuid(),
                UserId = User.Identity.GetUserId(),
                CartId = cart.Id,
                DateCreated = DateTime.Now,
                CreatedBy = User.Identity.GetUserId(),
                State = ShippingState.NotCheckedOut
            };
            _ShippingRepository.Add(shipD);

            // Modifying the number of product available in stock after the user confirms the order
            foreach (CartLineViewModel c in cartDto.Cart.Lines)
            {
                Product productToModify = _ProductRepository.GetSingle(x => x.Id == c.Product.Id);
                if(productToModify.Quantity >= c.Quantity)
                {
                    productToModify.Quantity -= c.Quantity;
                    _ProductRepository.Update(productToModify);
                }else
                {
                    return RedirectToAction("Index", "Checkout", new { errorMessage = "Stock insufficient for {0}", c.Product.Name  });
                }
            }
            // Modifying shipping state to pending
            ShippingDetail ship = _ShippingRepository.GetSingle(x => x.Id == shipD.Id );
            ship.State = ShippingState.Pending;
            ship.DateModified = DateTime.Now;
            ship.ModifiedBy = User.Identity.GetUserId();
            _ShippingRepository.Update(ship);

            // Sending Email to Administrator
            _OrderProcessor.ProcessOrder(cart, shipD);

            // Resetting cart content
            CartModelBinder.ResetBinding(ControllerContext);
            
            return View();
        }

    }
}