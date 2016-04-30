using System;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ShoppingCart.GeneralLib.CustomAttributs
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ValidateJsonAntiforgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            string serializedCookieToken = null;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[AntiForgeryConfig.CookieName];
            if (cookie != null)
            {
                serializedCookieToken = cookie.Value;
            }

            string serializedHeaderToken = HttpContext.Current.Request.Headers["__RequestVerificationToken"];
            AntiForgery.Validate(serializedCookieToken, serializedHeaderToken);
        }
    }
}