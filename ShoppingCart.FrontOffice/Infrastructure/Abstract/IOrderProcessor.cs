using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;
using ShoppingCart.Models.Models.User;

namespace ShoppingCart.Infrastructure.Abstract
{
    public interface IOrderProcessor
    {
        bool ProcessOrder(Cart cart, ShippingDetail shippingDetails, ApplicationUser user, IGenericRepository<Product> productRepository);
    }
}
