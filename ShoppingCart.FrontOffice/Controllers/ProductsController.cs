using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SharedFunctions.LambdaGenerator;
using ShoppingCart.Models;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;
using ShoppingCart.ViewModels;


namespace ShoppingCart.Controllers
{
    public class ProductsController : Controller
    {


        private IGenericRepository<Product> ProductRepository { get; }
        public int PageSize = 6;

        public ProductsController(IGenericRepository<Product> productRepository)
        {
            ProductRepository = productRepository;
            ProductRepository.AddNavigationProperties(p => p.Category);
            ProductRepository.AddNavigationProperty(p => p.Image);
        }





        // GET: Products
        public ActionResult List(string name = "", decimal price= 0, string category = "", int page = 1)
        {
            ProductsListViewModel productsViewModel;
            IList<Product> products = ProductRepository.GetList(p =>
                ((name != null && !name.Equals("")) ? p.Name.Equals(name) : true)
                && ((category != null && !category.Equals("")) ? p.Category.Name.Equals(category) : true)
                && ((price > 0) ? p.Price == price : true)
                );



            productsViewModel = new ProductsListViewModel()
            {

                Products = products,
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
            productsViewModel = new ProductDetailViewModel()
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

            ProductsListViewModel productsViewModel;
            IList<Product> products = ProductRepository.GetList(p =>
                ((name != null && !name.Equals("")) ? p.Name.Equals(name) : true)
                && ((category != null && !category.Equals("")) ? p.Category.Name.Equals(category) : true)
                && ((price > 0) ? p.Price == price : true)
                );



        productsViewModel = new ProductsListViewModel()
            {

                Products = products,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = products.Count()
                }
            };
            return Json(productsViewModel, JsonRequestBehavior.AllowGet); 
        }

    }
}
