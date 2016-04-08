using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;
using ShoppingCart.Models.Models.User;

namespace ShoppingCart.CommonController.Infrastructure.Abstract
{
    public interface IOrderProcessor
    {
        bool ProcessOrder(Cart cart, ShippingDetail shippingDetails, ApplicationUser user, IGenericRepository<Product> productRepository);
    }
}
