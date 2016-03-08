using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCart.Models.Models.Shopping;
using ShoppingCart.Models.Repositories.Interface;

namespace ShoppingCart.Controllers
{
    public class HomeController : Controller
    {
        private IGenericRepository<Product> ProductRepository { get; set; } 

        public HomeController(IGenericRepository<Product> productRepository)
        {
            ProductRepository = productRepository;
        }

        public ActionResult Index()
        {
            var products = ProductRepository.GetAll();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}