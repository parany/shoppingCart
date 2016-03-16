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

        public ProductsController(IGenericRepository<Product> productRepository)
        {
            ProductRepository = productRepository;
            ProductRepository.AddNavigationProperties(p => p.Category);
        }





        // GET: Products
        public ActionResult List(string name = "", decimal price= 0, string category = "")
        {
            ProductDetailViewModel productsViewModel;
            ProductExpression prExp = new ProductExpression();
            Expression exp = prExp.Compare("p", name, category, price);


            productsViewModel = new ProductDetailViewModel()
            {

                //Products = ProductRepository.GetList(p =>
                //                                          ((name != null && !name.Equals("")) ? p.Name.Equals(name) : true)
                //                                          && ((category != null && !category.Equals("")) ? p.Category.Name.Equals(category) : true)
                //                                          && ((price > 0) ? p.Price == price : true)
                //                                          )


                Products = ProductRepository.GetList(exp)

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

    }
}
