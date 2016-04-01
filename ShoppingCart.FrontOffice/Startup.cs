using Owin;
using Microsoft.Owin;

[assembly: OwinStartupAttribute(typeof(ShoppingCart.Startup))]
namespace ShoppingCart
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
