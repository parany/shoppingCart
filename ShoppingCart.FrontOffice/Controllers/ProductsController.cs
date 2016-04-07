using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ShoppingCart.CommonController.Controllers;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;
using ShoppingCart.ViewModels;

namespace ShoppingCart.Controllers
{
    public class ProductsController : BasicHomeController
    {
        public ProductsController(IGenericRepository<Product> productRepository, IGenericRepository<Category> categoryRepository) : base(productRepository, categoryRepository)
        {
        }

        
    }
}
