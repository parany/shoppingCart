using System;
using System.Net;
using System.Web.Mvc;

namespace ShoppingCart.BackOffice.Controllers
{
    public class HomeController : Controller
    {

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

    }
}
