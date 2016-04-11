using System.Web.Mvc;

namespace ShoppingCart.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        
        // GET: /Home/About
        // Action populating the about section of the web site
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        // GET: /Home/Contact
        // Action populating the contact section of the web site
        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page";

            return View();
        }
    }
}