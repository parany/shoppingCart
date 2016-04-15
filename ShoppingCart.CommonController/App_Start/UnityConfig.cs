using Unity.Mvc5;
using System.Web.Mvc;
using System.Data.Entity;
using System.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;
using Microsoft.AspNet.Identity.EntityFramework;

using ShoppingCart.Models;
using ShoppingCart.Models.Models.User;
using ShoppingCart.CommonController.Controllers;
using ShoppingCart.CommonController.Infrastructure.Email;
using ShoppingCart.CommonController.Infrastructure.Abstract;
using Microsoft.Owin.Security;
using System.Web;
using ShoppingCart.Models.Repositories.Interface;
using ShoppingCart.Models.Repositories.Concrete;
using ShoppingCart.Models.Models.Entities;

namespace ShoppingCart.CommonController
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            EmailSettings emailSettings = new EmailSettings { WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false") };
            container.RegisterType<IOrderProcessor, EmailOrderProcessor>(new InjectionConstructor(emailSettings));
            container.RegisterType<BasicAccountController>(new InjectionConstructor());

            container.RegisterType<IGenericRepository<Product>, GenericRepository<Product>>();
            container.RegisterType<IGenericRepository<Image>, GenericRepository<Image>>();
            container.RegisterType<IGenericRepository<Cart>, GenericRepository<Cart>>();
            container.RegisterType<IGenericRepository<CartLine>, GenericRepository<CartLine>>();
            container.RegisterType<IGenericRepository<Category>, GenericRepository<Category>>();
            container.RegisterType<IGenericRepository<ShippingDetail>, GenericRepository<ShippingDetail>>();
            container.RegisterType<IGenericRepository<Provider>, GenericRepository<Provider>>();
            container.RegisterType<ProductRepository, ProductRepository>();

            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>();
            container.RegisterType<IRoleStore<ApplicationRole, string>, RoleStore<ApplicationRole>>();
            container.RegisterType<DbContext, ShoppingCartDbContext>(new HierarchicalLifetimeManager());
            container.RegisterType<UserManager<ApplicationUser>>(new HierarchicalLifetimeManager());
            container.RegisterType<RoleManager<ApplicationRole>>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new HierarchicalLifetimeManager());
            container.RegisterType<IRoleStore<ApplicationRole, string>, RoleStore<ApplicationRole>>(new HierarchicalLifetimeManager());
            container.RegisterType<IAuthenticationManager>(new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}