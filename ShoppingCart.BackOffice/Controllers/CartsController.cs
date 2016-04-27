using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;
using ShoppingCart.CommonController.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCart.Controllers;

namespace ShoppingCart.BackOffice.Controllers
{
    [Authorize(Roles = "AllPermissions")]
    public class CartsController : BasicCartsController
    {
        public CartsController(IGenericRepository<Product> productRepository,
                               IGenericRepository<Cart> cartRepository,
                               IGenericRepository<ShippingDetail> shipRepository,
                               IGenericRepository<CartLine> cartlineRepository) : base(productRepository, cartRepository, shipRepository, cartlineRepository)
        {
        }
    }
}