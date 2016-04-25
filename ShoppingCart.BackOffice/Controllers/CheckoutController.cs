using ShoppingCart.BackOffice.ViewsModels;
using ShoppingCart.CommonController.Infrastructure.Abstract;
using ShoppingCart.CommonController.Infrastructure.Binders;
using ShoppingCart.CommonController.ViewModels;
using ShoppingCart.Controllers;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Models.User;
using ShoppingCart.Models.Repositories.Concrete;
using ShoppingCart.Models.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.BackOffice.Controllers
{
    public class CheckoutController : BasicCheckoutController
    {
        public CheckoutController(IGenericRepository<Cart> cartRepo,
                                  ProductRepository productRepo,
                                  IGenericRepository<CartLine> cartlineRepo,
                                  IGenericRepository<ShippingDetail> shipRepo,
                                  IOrderProcessor proc)
            : base(cartRepo, productRepo, cartlineRepo, shipRepo, proc)
        {
        }

        [Authorize]
        [HttpPost]
        public override ActionResult Order(CheckoutDTO cartDto)
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
                foreach (CommonController.ViewModels.CartLineViewModel cartl in cartDto.Cart.Lines)
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
                foreach (CommonController.ViewModels.CartLineViewModel c in cartDto.Cart.Lines)
                {

                    Product productToModify = _ProductRepository.GetSingle(x => x.Id == c.Product.Id);
                    Product productInStock = _ProductRepository.GetSingle(x => x.ProductReference == productToModify.ProductReference && x.Type == ProductType.ForSale);

                    if (productInStock != null) //InStock
                    {
                        if (productToModify.Quantity >= c.Quantity)
                        {
                            productToModify.Quantity -= c.Quantity;
                            productInStock.Quantity += c.Quantity;

                            _ProductRepository.Update(productInStock);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Checkout", new { errorMessage = "Stock insufficient for {0}, Please see availaible product in Product Details", c.Product.Name });
                        }
                    }
                    else // New
                    {
                        if (productToModify.Quantity >= c.Quantity)
                        {
                            Product newProduct = new Product
                            {
                                Id = Guid.NewGuid(),
                                Name = productToModify.Name,
                                Description = productToModify.Description,
                                Price = productToModify.Price,
                                Quantity = c.Quantity,
                                ImageId = productToModify.ImageId,
                                CategoryId = productToModify.CategoryId,
                                ProductReference = productToModify.ProductReference,
                                Type = ProductType.ForSale,
                                Providers = new List<Provider>(),
                                DateCreated = DateTime.Now,
                                CreatedBy = user.UserName
                            };

                            foreach(Provider pro in productToModify.Providers)
                            {
                                newProduct.Providers.Add(pro);
                            }

                            _ProductRepository.Add(newProduct);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Checkout", new { errorMessage = "Stock insufficient for {0}, Please see availaible product in Product Details", c.Product.Name });
                        }
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