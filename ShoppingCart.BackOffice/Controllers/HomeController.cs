using System.Web.Mvc;

namespace ShoppingCart.BackOffice.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class HomeController : Controller
    {

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

    }
}
