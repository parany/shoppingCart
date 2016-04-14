using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

using ShoppingCart.Models.Repositories.Interface;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Models.User;
using ShoppingCart.Models.Models.Payments;
using ShoppingCart.CommonController.Infrastructure.Abstract;
using ShoppingCart.CommonController.Infrastructure.Identity;
using ShoppingCart.CommonController.ViewModels;
using ShoppingCart.CommonController.Infrastructure.Binders;

namespace ShoppingCart.Controllers
{

    public class BasicCheckoutController : Controller
    {
        private IOrderProcessor _OrderProcessor;
        private IGenericRepository<Cart> _CartRepository { get; set; }
        private IGenericRepository<Product> _ProductRepository { get; set; }
        private IGenericRepository<CartLine> _CartLineRepository { get; set; }
        private IGenericRepository<ShippingDetail> _ShippingRepository { get; set; }

        private ApplicationUser CurrentUser { get { return UserManager.FindByName(HttpContext.User.Identity.Name); } }
        private ApplicationUserManager UserManager { get { return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); } }

        private ApplicationRoleManager RoleManager { get { return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>(); } }
        public BasicCheckoutController(IGenericRepository<Cart> cartRepo,
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
            _CartLineRepository.AddNavigationProperty(cl => cl.Cart);
            _ShippingRepository = shipRepo;
            _OrderProcessor = proc;
        }

        // GET: /Checkout/Index or /Checkout
        // Action for populating a summary of the order and associating the order to a customer
        [Authorize]
        public ActionResult Index(CartViewModel cartView, String errorMessage = "")
        {
            StansactionType transaction;

            ApplicationUser user = CurrentUser;
            if (RoleManager.RoleExists("Admin") && UserManager.IsInRole(user.Id, "Admin"))
            {
                transaction = StansactionType.Buying;
            }
            else
            {
                transaction = StansactionType.Selling;
            }
            // Creating ViewModel to pass to the view for rendering summary
            CheckoutDTO cdto = new CheckoutDTO
            {
                Cart = cartView,
                UserName = user.UserName,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                ErrorMessage = errorMessage,
                Payments = new Payments(),
                TransactionType = transaction
            };
            string path = Server.MapPath("~/App_Data/payment.xml");
            cdto.Payments.InitPaymentsList(path);
            return View(cdto);
        }

        // POST: /Checkout/Order
        // Action confirming the order
        [Authorize]
        [HttpPost]
        public ActionResult Order(CheckoutDTO cartDto)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = CurrentUser;

                // Creating Shipping Details for the order
                ShippingDetail shipD = new ShippingDetail
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    DateCreated = DateTime.Now,
                    CreatedBy = user.UserName,
                    Name = cartDto.UserName,
                    Address = cartDto.Address,
                    PhoneNumber = cartDto.PhoneNumber
                };
                _ShippingRepository.Add(shipD);

                // Creating a new cart in persistence
                Cart cart = new Cart
                {
                    Id = Guid.NewGuid(),
                    DateCreated = DateTime.Now,
                    CreatedBy = user.UserName,
                    UserId = user.Id,
                    ShippingDetailId = shipD.Id,
                    PaymentMethod = cartDto.PaymentsMethod,
                    State = ShippingState.Pending,
                    TransactionType = cartDto.TransactionType
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
                        CreatedBy = user.Id
                    };
                    _CartLineRepository.Add(cartline);

                    cart.CartLines.Add(cartline);
                }
                cart.DateModified = DateTime.Now;
                cart.ModifiedBy = user.Id;
                _CartRepository.Update(cart);

                // Modifying the number of product available in stock after the user confirms the order
                foreach (CartLineViewModel c in cartDto.Cart.Lines)
                {
                    Product productToModify = _ProductRepository.GetSingle(x => x.Id == c.Product.Id);
                    if (productToModify.Quantity >= c.Quantity)
                    {
                        if (cartDto.TransactionType == StansactionType.Selling)
                            productToModify.Quantity -= c.Quantity;
                        if (cartDto.TransactionType == StansactionType.Buying)
                            productToModify.Quantity += c.Quantity;

                        _ProductRepository.Update(productToModify);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Checkout", new { errorMessage = "Stock insufficient for {0}, Please see availaible product in Product Details", c.Product.Name });
                    }
                }
                // Modifying cart state to pending
                cart.State = ShippingState.Pending;
                cart.DateModified = DateTime.Now;
                cart.ModifiedBy = user.Id;
                _CartRepository.Update(cart);

                // Sending Email to Administrator
                bool result = _OrderProcessor.ProcessOrder(cart, shipD, user, _ProductRepository);

                // Resetting cart content
                CartModelBinder.ResetBinding(ControllerContext);

                return View(result);
            }
            else
            {
                return RedirectToAction("Index", "Checkout", cartDto);
            }
        }

    }
}