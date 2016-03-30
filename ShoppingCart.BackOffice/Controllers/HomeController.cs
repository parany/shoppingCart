using System;
using System.Net;
using System.Web.Mvc;

namespace ShoppingCart.BackOffice.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

    }
}
