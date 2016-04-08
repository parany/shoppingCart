using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ShoppingCart.CommonController.ViewModels.Home;
using ShoppingCart.CommonController.Meta_Entity;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;

namespace ShoppingCart.CommonController.Controllers
{
    public class BasicHomeController : Controller
    {
        private IGenericRepository<Product> _ProductRepository;
        private IGenericRepository<Category> _CategoryRepository;

        public BasicHomeController(IGenericRepository<Product> productRepository, IGenericRepository<Category> categoryRepository)
        {
            _ProductRepository = productRepository;
            _ProductRepository.AddNavigationProperty(c => c.Category);
            _ProductRepository.AddNavigationProperty(c => c.Image);
            _CategoryRepository = categoryRepository;
        }
        public virtual ActionResult Index()
        {
            IList<Product> products = _ProductRepository.GetAll();
            IList<Category> categories = _CategoryRepository.GetAll();
            ProductsListViewModel viewModel = new ProductsListViewModel
            {
                Products = products
                             .OrderByDescending(p => p.DateCreated),
                Categories = categories
            };

            return View(viewModel);
        }

        public virtual ActionResult AllList()
        {
            IList<Product> products = _ProductRepository.GetAll();
            IList<Category> categories = _CategoryRepository.GetAll();

            List<MetaProduct> resPr = new List<MetaProduct>();

            foreach (Product p in products)
            {
                MetaProduct mp = new MetaProduct();
                mp.Name = p.Name;
                mp.Category = p.Category.Name;
                mp.Description = p.Description;
                mp.Image = p.Image;
                mp.Price = p.Price.ToString("c");
                mp.Quantity = p.Quantity;
                mp.ID = p.Id;
                resPr.Add(mp);
            }

            List<MetaCategory> resCat = new List<MetaCategory>();
            foreach (Category c in categories)
            {
                MetaCategory mc = new MetaCategory();
                mc.Name = c.Name;
                resCat.Add(mc);
            }


            List<Object> list = new List<Object>();
            list.Add(resCat);
            list.Add(resPr);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult GetList(string name = "", decimal price = 0, string category = "")
        {
            var products = _ProductRepository.GetList(p =>
                (name != null && !name.Equals("") ? p.Name.Equals(name) : true)
                && (category != null && !category.Equals("") ? p.Category.Name.Equals(category) : true)
                && (price > 0 ? p.Price == price : true)
                );

            List<MetaProduct> resultList = new List<MetaProduct>();

            foreach (Product p in products)
            {
                MetaProduct mp = new MetaProduct();
                mp.Name = p.Name;
                mp.Category = p.Category.Name;
                mp.Description = p.Description;
                mp.Image = p.Image;
                mp.Price = p.Price.ToString("c");
                mp.Quantity = p.Quantity;
                mp.ID = p.Id;
                resultList.Add(mp);
            }
            return Json(resultList);
        }

        public virtual ActionResult List(string name = "", decimal price = 0, string category = "", int page = 1)
        {
            ProductsListViewModel productsViewModel;
            var products = _ProductRepository.GetList(p =>
                (name != null && !name.Equals("") ? p.Name.Equals(name) : true)
                && (category != null && !category.Equals("") ? p.Category.Name.Equals(category) : true)
                && (price > 0 ? p.Price == price : true)
                );
            var categories = _CategoryRepository.GetAll();


            productsViewModel = new ProductsListViewModel
            {
                Products = products,
                Categories = categories
            };
            return View(productsViewModel);
        }

        public virtual JsonResult ListUpdate(string name = "", decimal price = 0, string category = "", int page = 1)
        {

            var products = _ProductRepository.GetList(p =>
                (name != null && !name.Equals("") ? p.Name.Equals(name) : true)
                && (category != null && !category.Equals("") ? p.Category.Name.Equals(category) : true)
                && (price > 0 ? p.Price == price : true)
                );

            List<MetaProduct> resultList = new List<MetaProduct>();

            foreach (Product p in products)
            {
                MetaProduct mp = new MetaProduct();
                mp.Name = p.Name;
                mp.Category = p.Category.Name;
                mp.Description = p.Description;
                mp.Image = p.Image;
                mp.Price = p.Price.ToString("c");
                mp.Quantity = p.Quantity;
                mp.ID = p.Id;
                resultList.Add(mp);
            }


            return Json(resultList);
        }
        public virtual JsonResult AllHint()
        {
            var products = _ProductRepository.GetAll();
            var categories = _CategoryRepository.GetAll();


            List<MetaProduct> resPr = new List<MetaProduct>();

            foreach (Product p in products)
            {
                MetaProduct mp = new MetaProduct();
                mp.Name = p.Name;
                mp.Category = p.Category.Name;
                mp.Description = p.Description;
                mp.Image = p.Image;
                mp.Price = p.Price.ToString("c");
                mp.Quantity = p.Quantity;
                mp.ID = p.Id;
                resPr.Add(mp);
            }

            List<MetaCategory> resCat = new List<MetaCategory>();
            foreach (Category c in categories)
            {
                MetaCategory mc = new MetaCategory();
                mc.Name = c.Name;
                resCat.Add(mc);
            }


            List<Object> list = new List<Object>();
            list.Add(resCat);
            list.Add(resPr);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public virtual IGenericRepository<Product> GetProductRepo()
        {
            return _ProductRepository;
        }

        public virtual IGenericRepository<Category> GetCategoryRepo()
        {
            return _CategoryRepository;
        }
        // Action for populating the details of a product
        [HttpPost]
        public ViewResult Details(Guid productId, string returnUrl)
        {
            // Getting the product to populate details
            IGenericRepository<Product> repo = GetProductRepo();
            Product product = repo.GetSingle(p => p.Id == productId);

            // Passing the return Url to view
            ViewData["ReturnUrl"] = returnUrl;

            //returning view
            return View(product);
        }
    }
}