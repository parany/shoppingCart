using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart.Controllers
{
    public class HomeController : Controller
    {
        private IGenericRepository<Product> ProductRepository { get; }
        public int PageSize = 10;

        public HomeController(IGenericRepository<Product> productRepository)
        {
            ProductRepository = productRepository;
        }

        public ViewResult Index(int page = 1)
        {
            IList<Product> products = ProductRepository.GetAll();
            return View(products);
            /*
            return View(products
                .OrderBy(p => p.Id)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
               );
               */
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