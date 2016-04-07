using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ShoppingCart.CommonController.Controllers;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;
using ShoppingCart.ViewModels;

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