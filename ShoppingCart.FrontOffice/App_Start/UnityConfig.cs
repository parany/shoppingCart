using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Practices.Unity;
using ShoppingCart.Models.Models.Shopping;
using ShoppingCart.Models.Models.User;
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
			container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>();

			DependencyResolver.SetResolver(new UnityDependencyResolver(container));
		}
	}
}