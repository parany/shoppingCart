using ShoppingCart.CommonController.Infrastructure.Abstract;
using ShoppingCart.Controllers;
using ShoppingCart.Models.Models.Entities;
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
                                  IGenericRepository<Product> productRepo,
                                  IGenericRepository<CartLine> cartlineRepo,
                                  IGenericRepository<ShippingDetail> shipRepo,
                                  IOrderProcessor proc)
            : base(cartRepo, productRepo, cartlineRepo, shipRepo, proc)
        {
        }
    }
}