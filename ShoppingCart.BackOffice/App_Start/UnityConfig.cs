using Microsoft.Practices.Unity;
using System.Web.Mvc;
using Unity.Mvc5;

using ShoppingCart.Controllers;
using ShoppingCart.Models;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Models.User;
using ShoppingCart.Models.Repositories.Concrete;
using ShoppingCart.Models.Repositories.Interface;
using ShoppingCart.Services.Interface;
using ShoppingCart.Services.Implementation;
using ShoppingCart.Models.Log;

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
			container.RegisterType<IGenericRepository<Provider>, GenericRepository<Provider>>();
			container.RegisterType<IProvidersService, ProvidersService>();
			container.RegisterType<IGenericRepository<ChangeTracking>, GenericRepository<ChangeTracking>>();

			DependencyResolver.SetResolver(new UnityDependencyResolver(container));
		}
	}
}
