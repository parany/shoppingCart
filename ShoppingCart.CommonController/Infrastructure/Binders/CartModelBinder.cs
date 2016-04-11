using System.Web.Mvc;
using ShoppingCart.CommonController.ViewModels;

namespace ShoppingCart.CommonController.Infrastructure.Binders
{
    public class CartModelBinder : IModelBinder
    {
        private const string sessionKey = "Cart";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            
            CartViewModel cart = null;
            if (controllerContext.HttpContext.Session != null)
            {
                cart = (CartViewModel)controllerContext.HttpContext.Session[sessionKey];
            }
            
            if (cart == null)
            {
                cart = new CartViewModel();
                if (controllerContext.HttpContext.Session != null)
                {
                    controllerContext.HttpContext.Session[sessionKey] = cart;
                }
            }
            
            return cart;
        }

        public static void ResetBinding(ControllerContext controllerContext)
        {
            controllerContext.HttpContext.Session[sessionKey] = null;
        }
    }
}