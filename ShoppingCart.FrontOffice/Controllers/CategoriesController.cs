using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShoppingCart.Models;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;
using ShoppingCart.ViewModels;

namespace ShoppingCart.Controllers
{
    public class CategoriesController : Controller
    {

        private IGenericRepository<Category> CategoryRepository { get; }

        public CategoriesController(IGenericRepository<Category> categoryRepository, IGenericRepository<Product> productsRepository)
        {
            CategoryRepository = categoryRepository;
            CategoryRepository.AddNavigationProperties(c => c.Products);
        }


        // GET: Categories
        public ActionResult Index(string categoryName)
        {
            CategoriesViewModel categoriesViewModel;
            if (categoryName != null)
            {
                categoriesViewModel = new CategoriesViewModel()
                {
                    Categories = CategoryRepository.GetAll(),
                    SelectedCategories = CategoryRepository.GetList(c => c.Name == categoryName)
                };
            }
            else
            {
                categoriesViewModel = new CategoriesViewModel()
                {
                    Categories = CategoryRepository.GetAll(),
                    SelectedCategories = null
                };
            }
            
            return View(categoriesViewModel);
        }

        public ActionResult Details(string name)
        {
            CategoriesViewModel categoriesViewModel;
            if (name != null)
            {
                categoriesViewModel = new CategoriesViewModel()
                {
                    SelectedCategories = CategoryRepository.GetList(c => c.Name == name),
                    Categories = null
                };
            }
            else
            {
                categoriesViewModel = new CategoriesViewModel()
                {
                    SelectedCategories = null,
                    Categories = null
                };
            }
            return View(categoriesViewModel);
        }



    }
}
