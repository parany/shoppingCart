using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;
using ShoppingCart.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ShoppingCart.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private IGenericRepository<Product> _ProductRepository { get; set; } 
        public int PageSize = 3;

        public HomeController(IGenericRepository<Product> productRepository)
        {
            _ProductRepository = productRepository;
            _ProductRepository.AddNavigationProperty(c => c.Category);
            _ProductRepository.AddNavigationProperty(c => c.Image);
        }

        // GET: /Home/Index or /Home or /
        // Action for populating the product creating in the last week
        public ViewResult Index(int page = 1)
        {
            // Retrieving Product created during the last week
            //DateTime lastWeek = DateTime.Today.AddDays(-7);
            //IList<Product> products = _ProductRepository.GetList(p => (p.DateCreated > lastWeek));
            IList<Product> products = _ProductRepository.GetAll();

            // Building viewModel containing products and paging information to pass to the view
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
            //returning view
            return View(viewModel);
        }

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

        // POST: /Home/Details
        // Action for populating the details of a product
        [HttpPost]
        public ViewResult Details(Guid productId, string returnUrl)
        {
            // Getting the product to populate details
            Product product = _ProductRepository.GetSingle(p => p.Id == productId);

            // Passing the return Url to view
            ViewData["ReturnUrl"] = returnUrl;

            //returning view
            return View(product);
        }
    }
}