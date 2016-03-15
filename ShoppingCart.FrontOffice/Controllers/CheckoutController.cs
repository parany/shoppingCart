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
        private IGenericRepository<ShippingDetail> _ShippingRepository { get; set; }
        private IGenericRepository<CartLine> _CartLineRepo { get; set; }

        public CheckoutController(IGenericRepository<Cart> cartRepository,IGenericRepository<ShippingDetail> shipRepo, IGenericRepository<CartLine> cRepo)
        {
            _CartRepository = cartRepository;
            _CartRepository.AddNavigationProperties(ca => ca.CartLines);
            _ShippingRepository = shipRepo;
            _CartLineRepo = cRepo;
            _CartLineRepo.AddNavigationProperty(cl => cl.Product);
            _CartLineRepo.AddNavigationProperty(clr => clr.Cart);
        }

        [Authorize]
        // GET: Checkout
        public ActionResult Index(CartViewModel cartView)
        {
            Cart cart = new Cart {
                Id=Guid.NewGuid()
            };
            _CartRepository.Add(cart);

            foreach (CartLineViewModel cartl in cartView.Lines)
            {
                CartLine cartline = new CartLine
                {
                    Id = Guid.NewGuid(),
                    Quantity = cartl.Quantity,
                    CartId = cart.Id,
                    ProductId = cartl.Product.Id,
                };
            _CartLineRepo.Add(cartline);
            cart.CartLines.Add(cartline);
            }

            _CartRepository.Update(cart);

            CartDTO cdto = new CartDTO
            {
                Cart = _CartRepository.GetSingle(x => x.Id == cart.Id),
                UserId = User.Identity.GetUserId(),
                UserName = User.Identity.GetUserName()
            };
            return View(cdto);
        }

        [HttpPost]
        public RedirectToRouteResult Order(CartDTO cartDto)
        {

            ShippingDetail shipD = new ShippingDetail
            {
                UserId = cartDto.UserId,
                CartId = cartDto.Cart.Id,
                Cart = cartDto.Cart,
                DateCreated = DateTime.Now,
                CreatedBy = User.Identity.Name,
                ModifiedBy = User.Identity.Name,
                DateModified = DateTime.Now,
                Id = Guid.NewGuid()

            };
            _ShippingRepository.Add(shipD);


            CartModelBinder.UnBindModel(ControllerContext);

            _CartRepository.Delete(cartDto.Cart);
            return RedirectToRoute("/");
        }


    }
}