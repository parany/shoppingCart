using System.Web.Mvc;
using Microsoft.Practices.Unity;
using ShoppingCart.Models.Models.Shopping;
using ShoppingCart.Models.Repositories.Concrete;
using ShoppingCart.Models.Repositories.Interface;
using Unity.Mvc5;

namespace ShoppingCart
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<IGenericRepository<Product>, GenericRepository<Product>>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}