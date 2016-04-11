using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using System.Web;
using System.Web.Mvc;
using Unity.Mvc5;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Concrete;
using ShoppingCart.Models.Repositories.Interface;

namespace ShoppingCart
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterType<IGenericRepository<Product>, GenericRepository<Product>>();
            container.RegisterType<IGenericRepository<Image>, GenericRepository<Image>>();
            container.RegisterType<IGenericRepository<Cart>, GenericRepository<Cart>>();
            container.RegisterType<IGenericRepository<CartLine>, GenericRepository<CartLine>>();
            container.RegisterType<IGenericRepository<Category>, GenericRepository<Category>>();
            container.RegisterType<IGenericRepository<ShippingDetail>, GenericRepository<ShippingDetail>>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}