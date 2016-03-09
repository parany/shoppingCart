using ShoppingCart.Models.Models.Entities;
using ShoppingCart.ViewModels;
using ShoppingCart.Models.Repositories.Interface;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;

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
            DateTime lastWeek = DateTime.Now.AddDays(7);

            IList<Product> products = ProductRepository.GetList(p => (p.DateCreated < lastWeek));

            ProductsListViewModel viewModel = new ProductsListViewModel
            {
                Products = products
                             .OrderBy(p => p.DateCreated)
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
    }
}