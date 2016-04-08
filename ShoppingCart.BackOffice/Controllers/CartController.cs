using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;
using ShoppingCart.BackOffice.ViewsModels;
using ShoppingCart.GeneralLib.CustomAttributs;

namespace ShoppingCart.BackOffice.Controllers
{
    [Authorize(Roles = "Admin")]
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
        public ActionResult Index(ShippingState id = ShippingState.Pending)
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
                    Id = cart.Id,
                    UserName = cart.User.UserName,
                    CartLines = varLines,
                    ShippingDetail = shippingDetailViewModel,
                    ShippingState = cart.State,
                    DateCreated = cart.DateCreated
                });
            }

            ViewBag.ShippingStateCounts = new Dictionary<ShippingState, int>();

            ViewBag.ShippingStateCounts.Add(ShippingState.Pending,
                                            cartsViewModels.Count(x => x.ShippingState == ShippingState.Pending));
            ViewBag.ShippingStateCounts.Add(ShippingState.Delivered,
                                            cartsViewModels.Count(x => x.ShippingState == ShippingState.Delivered));
            ViewBag.ShippingStateCounts.Add(ShippingState.Canceled,
                                            cartsViewModels.Count(x => x.ShippingState == ShippingState.Canceled));

            IEnumerable<CartsViewModel> cartsViewModelsReturn = cartsViewModels.Where(x => x.ShippingState == ShippingState.Pending);

            if (id == ShippingState.Delivered)
            {
                cartsViewModelsReturn = cartsViewModels.Where(x => x.ShippingState == ShippingState.Delivered);
            }
            else if (id == ShippingState.Canceled)
            {
                cartsViewModelsReturn = cartsViewModels.Where(x => x.ShippingState == ShippingState.Canceled);
            }

            ViewBag.ActiveTab = id;
            
            return View(cartsViewModelsReturn);
        }

        [HttpPost]
        [ValidateJsonAntiforgeryToken]
        public ActionResult Index(ChangeStateViewModel cartIdandState)
        {            
            var cart = CartRepository.GetSingle(x => x.Id == cartIdandState.Id);
            cart.State = cartIdandState.State;
            if (cart != null)
            {
                CartRepository.Update(cart);
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}
