using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
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
        public ActionResult List(string name = "", decimal price= 0, string category = "")
        {


            var funStringCompare = new Func<string, string, bool>((string a, string b) =>
            {
                if (!String.IsNullOrWhiteSpace(b) && !String.IsNullOrWhiteSpace(a))
                {
                    return a.Equals(b);
                }
                else
                {
                    return true;
                }
            });

            var funDecimalCompare = new Func<decimal, decimal, bool>((decimal dec1, decimal dec2) =>
            {
                if (dec1 > 0 && dec2 > 0)
                {
                    return dec1 == dec2;
                }
            else
                {
                    return true;
                }
            });


            var productFun = new Func<Product, string, string, decimal, bool>(
                (Product p, string s, string s1, decimal d) =>
                {
                    return funStringCompare(p.Name, s) && funStringCompare(p.Category.Name, s1) && funDecimalCompare(p.Price, d);
                });

            ProductDetailViewModel productsViewModel;
            productsViewModel = new ProductDetailViewModel()
            {
                Products = ProductRepository.GetList(p =>
                                                          ((name != null && !name.Equals("")) ? p.Name.Equals(name): true)
                                                          && ((category != null && !category.Equals("")) ? p.Category.Name.Equals(category) : true)
                                                          && ((price > 0) ? p.Price == price : true)
                                                          )
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
