using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Practices.Unity;
using ShoppingCart.Controllers;
using ShoppingCart.Models;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Models.User;
using ShoppingCart.Models.Repositories.Concrete;
using ShoppingCart.Models.Repositories.Interface;
using System.Data.Entity;
using System.Web.Mvc;
using Unity.Mvc5;

namespace ShoppingCart.BackOffice
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

			container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>();
			container.RegisterType<DbContext, ShoppingCartDbContext>(new HierarchicalLifetimeManager());
			container.RegisterType<UserManager<ApplicationUser>>(new HierarchicalLifetimeManager());
			container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new HierarchicalLifetimeManager());
			container.RegisterType<AccountController>(new InjectionConstructor());

			DependencyResolver.SetResolver(new UnityDependencyResolver(container));
		}
	}
}
