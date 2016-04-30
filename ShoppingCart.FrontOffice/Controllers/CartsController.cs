using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;

namespace ShoppingCart.Controllers
{
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