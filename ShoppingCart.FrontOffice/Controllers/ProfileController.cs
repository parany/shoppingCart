using System.Web.Mvc;

namespace ShoppingCart.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile/ChangeTheme
        public ActionResult ChangeTheme(string themename)
        {
            Session["CssTheme"] = themename;
            if (Request.UrlReferrer != null)
            {
                var returnUrl = Request.UrlReferrer.ToString();
                return new RedirectResult(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}