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

namespace ShoppingCart.Controllers
{
    
    public class CheckoutController : Controller
    {
        private IGenericRepository<Cart> _CartRepository { get; set; }
        private IGenericRepository<Product> _ProductRepository { get; set; }
        private IGenericRepository<CartLine> _CartLineRepository { get; set; }
        private IGenericRepository<ShippingDetail> _ShippingRepository { get; set; }

        public CheckoutController(IGenericRepository<Cart> cartRepo,
                                  IGenericRepository<Product> productRepo,
                                  IGenericRepository<CartLine> cartlineRepo,
                                  IGenericRepository<ShippingDetail> shipRepo)
        {
            _ProductRepository = productRepo;
            _CartRepository = cartRepo;
            _CartRepository.AddNavigationProperties(ca => ca.CartLines);
            _CartLineRepository = cartlineRepo;
            _CartLineRepository.AddNavigationProperty(cl => cl.Product);
            _CartLineRepository.AddNavigationProperty(clr => clr.Cart);
            _ShippingRepository = shipRepo;
        }


        [Authorize]
        public ActionResult Index(CartViewModel cartView, String errorMessage = "")
        {
            if(cartView.Lines.Count() == 0)
            {
                return RedirectToAction("Index", "Carts", new { errorMessage = "Empty Cart! Please fill the cart before checking out." });
            }
            else
            {
                Cart cart = new Cart
                {
                    Id = Guid.NewGuid()
                };
                _CartRepository.Add(cart);

                foreach (CartLineViewModel cartl in cartView.Lines)
                {
                    CartLine cartline = new CartLine
                    {
                        Id = Guid.NewGuid(),
                        CartId = cart.Id,
                        ProductId = cartl.Product.Id,
                        Quantity = cartl.Quantity
                    };
                    _CartLineRepository.Add(cartline);
                    cart.CartLines.Add(cartline);
                }
                _CartRepository.Update(cart);

                CartDTO cdto = new CartDTO
                {
                    Cart = cartView,
                    CartId = cart.Id,
                    UserId = User.Identity.GetUserId(),
                    UserName = User.Identity.GetUserName(),
                    ErrorMessage = errorMessage
                };
                return View(cdto);
            }
           
        }

        [HttpPost]
        public ActionResult Order(CartDTO cartDto)
        {

            ShippingDetail shipD = new ShippingDetail
            {
                Id = Guid.NewGuid(),
                UserId = cartDto.UserId,
                CartId = cartDto.CartId
            };
            _ShippingRepository.Add(shipD);

            foreach(CartLineViewModel c in cartDto.Cart.Lines)
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

            CartModelBinder.ResetBinding(ControllerContext);
            
            return View();
        }
    }
}