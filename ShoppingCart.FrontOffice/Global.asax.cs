using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ShoppingCart.Infrastructure.Binders;
using ShoppingCart.ViewModels;

namespace ShoppingCart
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            UnityConfig.RegisterComponents();
            ModelBinders.Binders.Add(typeof(CartViewModel), new CartModelBinder());

        }
    }
}
