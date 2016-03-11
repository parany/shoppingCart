using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShoppingCart.Models;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;
using ShoppingCart.ViewModels;

namespace ShoppingCart.Controllers
{
    public class ProductsController : Controller
    {


        private IGenericRepository<Product> ProductRepository { get; }

        public ProductsController(IGenericRepository<Product> productRepository)
        {
            ProductRepository = productRepository;
            ProductRepository.AddNavigationProperties(p => p.Category);
        }





        // GET: Products
        public ActionResult List(string name = "", int ID = 0, decimal price= 0, string category = "")
        {
            /*
            Func<Product, bool> condition = p =>
            {
                return (string.IsNullOrEmpty(name) || p.Name.Equals(name))
                       && (string.IsNullOrEmpty(name) || p.Name.Equals(name));
            }
            ProductDetailViewModel productList = new ProductDetailViewModel()
            {
                Products = ProductRepository.GetList(Expression)
            }*/
            return View();
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

    }
}
