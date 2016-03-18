using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;
using ShoppingCart.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ShoppingCart.Controllers
{
    public class HomeController : Controller
    {
        private IGenericRepository<Product> _ProductRepository { get; set; } 
        public int PageSize = 6;

        public HomeController(IGenericRepository<Product> productRepository)
        {
            _ProductRepository = productRepository;
            _ProductRepository.AddNavigationProperty(c => c.Category);
        }

        public ViewResult Index(int page = 1)
        {
            DateTime lastWeek = DateTime.Now.AddDays(0);

            IList<Product> products = _ProductRepository.GetList(p => (p.DateCreated < lastWeek));

            ProductsListViewModel viewModel = new ProductsListViewModel
            {
                Products = products
                             .OrderByDescending(p => p.DateCreated)
                             .Skip((page - 1) * PageSize)
                             .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = products.Count()
                }
            };
            return View(viewModel);
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

        [HttpPost]
        public ViewResult Details(Guid productId, string returnUrl)
        {
            Product product = _ProductRepository.GetSingle(p => p.Id == productId);
            ViewData["ReturnUrl"] = returnUrl;
            return View(product);
        }
    }
}