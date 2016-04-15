using ShoppingCart.CommonController.Infrastructure.Abstract;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Concrete;
using ShoppingCart.Models.Repositories.Interface;

namespace ShoppingCart.Controllers
{
    public class CheckoutController : BasicCheckoutController
    {
        public CheckoutController(IGenericRepository<Cart> cartRepo
                                , ProductRepository productRepo, 
                                IGenericRepository<CartLine> cartlineRepo, 
                                IGenericRepository<ShippingDetail> shipRepo, 
            
            
            IOrderProcessor proc) : base(cartRepo, productRepo, cartlineRepo, shipRepo, proc)
        {
        }
    }
}