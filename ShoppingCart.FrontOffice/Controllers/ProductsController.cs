using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;
using ShoppingCart.ViewModels;

namespace ShoppingCart.Controllers
{
    public class ProductsController : Controller
    {
        private IGenericRepository<Product> ProductRepository { get; }
        private IGenericRepository<Category> CategoryRepository { get; }
        public int PageSize = 6;

        private class Meta_product
        {
            public Guid ID;
            public string Name { get; set; }

            public string Description { get; set; }

            public decimal Price { get; set; }

            public int Quantity { get; set; }

            public Image Image { get; set; }

            public string Category { get; set; }
        }

        public ProductsController(IGenericRepository<Product> productRepository, IGenericRepository<Category> categoryRepository)
        {
            ProductRepository = productRepository;
            ProductRepository.AddNavigationProperties(p => p.Category);
            ProductRepository.AddNavigationProperty(p => p.Image);

            CategoryRepository = categoryRepository;
        }


        // GET: Products
        public ActionResult List(string name = "", decimal price = 0, string category = "", int page = 1)
        {
            ProductsListViewModel productsViewModel;
            var products = ProductRepository.GetList(p =>
                (name != null && !name.Equals("") ? p.Name.Equals(name) : true)
                && (category != null && !category.Equals("") ? p.Category.Name.Equals(category) : true)
                && (price > 0 ? p.Price == price : true)
                );
            var categories = CategoryRepository.GetAll();


            productsViewModel = new ProductsListViewModel
            {
                Products = products,
                Categories = categories,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = products.Count()
                }
            };
            return View(productsViewModel);
        }

        // GET: Products/Details/5
        public ActionResult Details(string name)
        {
            ProductDetailViewModel productsViewModel;
            productsViewModel = new ProductDetailViewModel
            {
                Products = ProductRepository.GetList(p => p.Name == name)
            };
            return View(productsViewModel);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ListUpdate(string name = "", decimal price = 0, string category = "", int page = 1)
        {

            var products = ProductRepository.GetList(p =>
                (name != null && !name.Equals("") ? p.Name.Equals(name) : true)
                && (category != null && !category.Equals("") ? p.Category.Name.Equals(category) : true)
                && (price > 0 ? p.Price == price : true)
                );

            List<Meta_product> resultList = new List<Meta_product>();

            foreach (Product p in products)
            {
                Meta_product mp = new Meta_product();
                mp.Name = p.Name;
                mp.Category = p.Category.Name;
                mp.Description = p.Description;
                mp.Image = p.Image;
                mp.Price = p.Price;
                mp.Quantity = p.Quantity;
                mp.ID = p.Id;
                resultList.Add(mp);
            }
            var output = JsonConvert.SerializeObject(resultList);

            return Json(resultList);
        }
    }
}
