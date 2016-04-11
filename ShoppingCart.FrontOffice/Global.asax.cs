using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using ShoppingCart.Models.Models.Initializer;
using ShoppingCart.CommonController.ViewModels;
using ShoppingCart.CommonController.Infrastructure.Binders;

namespace ShoppingCart
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new ShoppingCartDbInitializer());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            UnityConfig.RegisterComponents();
            CommonController.UnityConfig.RegisterComponents();
            ModelBinders.Binders.Add(typeof(CartViewModel), new CartModelBinder());

        }
    }
}
