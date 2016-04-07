using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;
using ShoppingCart.ViewModels;
using ShoppingCart.CommonController.Controllers;

namespace ShoppingCart.Controllers
{
    public class ProductsController : BasicHomeController
    {
        public ProductsController(IGenericRepository<Product> productRepository, IGenericRepository<Category> categoryRepository) : base(productRepository, categoryRepository)
        {
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
