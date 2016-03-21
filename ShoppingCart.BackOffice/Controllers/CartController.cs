using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;
using ShoppingCart.BackOffice.ViewsModels;

namespace ShoppingCart.BackOffice.Controllers
{
    public class CartController : Controller
    {
        private IGenericRepository<Cart> CartRepository;
        private IGenericRepository<Product> ProductRepository;
        private IGenericRepository<ShippingDetail> ShippingDetailRepository;
        public CartController(IGenericRepository<Cart> cartRepository,
                              IGenericRepository<Product> productRepository,
                              IGenericRepository<ShippingDetail> shippingDetailRepository)
        {
            CartRepository = cartRepository;
            ProductRepository = productRepository;
            ShippingDetailRepository = shippingDetailRepository;
            CartRepository.AddNavigationProperties(x => x.CartLines, x => x.User, x => x.ShippingDetail);
            
        }
        // GET: Cart
        public ActionResult Index()
        {
            var carts = CartRepository.GetAll();
            var cartsViewModels = new List<CartsViewModel>();
            
            foreach (var cart in carts)
            {
                var varLines = new List<CartLineViewModel>();
                foreach (var cartLine in cart.CartLines)
                {
                    varLines.Add(new CartLineViewModel
                    {
                        Product = ProductRepository.GetSingle(x => x.Id == cartLine.ProductId),
                        Quantity = cartLine.Quantity
                    });
                }
                var shippingDetail = ShippingDetailRepository.GetSingle(x => x.Id == cart.ShippingDetailId);
                var shippingDetailViewModel = new ShippingDetailViewModel
                {
                    Name = shippingDetail.Name,
                    Address = shippingDetail.Address,
                    PhoneNumber = shippingDetail.PhoneNumber
                };
                cartsViewModels.Add(new CartsViewModel
                {
                    UserName = cart.User.UserName,
                    CartLines = varLines,
                    ShippingDetail = shippingDetailViewModel
                });
            }
            
            return View(cartsViewModels);
        }
    }
}