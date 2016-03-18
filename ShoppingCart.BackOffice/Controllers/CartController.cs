using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;

namespace ShoppingCart.BackOffice.Controllers
{
    public class CartController : Controller
    {
        private IGenericRepository<Cart> CartRepository;
        public CartController(IGenericRepository<Cart> cartRepository)
        {
            CartRepository = cartRepository;
            CartRepository.AddNavigationProperties(x => x.CartLines, x => x.ShippingDetail);
        }
        // GET: Cart
        public ActionResult Index()
        {
            var carts = CartRepository.GetAll();
            return View(carts);
        }
    }
}