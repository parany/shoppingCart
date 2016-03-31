using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Unity.Mvc5;
using System.Configuration;

using ShoppingCart.Controllers;
using ShoppingCart.Models;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Models.User;
using ShoppingCart.Models.Repositories.Concrete;
using ShoppingCart.Models.Repositories.Interface;
using ShoppingCart.Infrastructure.Concrete;
using ShoppingCart.Infrastructure.Abstract;

namespace ShoppingCart
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            EmailSettings emailSettings = new EmailSettings{WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")};

            container.RegisterType<IGenericRepository<Product>, GenericRepository<Product>>();
            container.RegisterType<IGenericRepository<Image>, GenericRepository<Image>>();
            container.RegisterType<IGenericRepository<Cart>, GenericRepository<Cart>>();
            container.RegisterType<IGenericRepository<CartLine>, GenericRepository<CartLine>>();
            container.RegisterType<IGenericRepository<Category>, GenericRepository<Category>>();
            container.RegisterType<IGenericRepository<ShippingDetail>, GenericRepository<ShippingDetail>>();
            container.RegisterType<IOrderProcessor, EmailOrderProcessor>(new InjectionConstructor(emailSettings));

            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>();
            container.RegisterType<DbContext, ShoppingCartDbContext>(new HierarchicalLifetimeManager());
            container.RegisterType<UserManager<ApplicationUser>>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new HierarchicalLifetimeManager());
            container.RegisterType<AccountController>(new InjectionConstructor());
            container.RegisterType<IAuthenticationManager>(new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}