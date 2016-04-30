using System.Web.Mvc;

namespace ShoppingCart.BackOffice.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Roles = "AllPermissions, Read, ReadWrite")]
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

    }
}
