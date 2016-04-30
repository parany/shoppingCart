using System;
using System.Web.Mvc;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;
using ShoppingCart.CommonController.Controllers;

namespace ShoppingCart.Controllers
{
    public class ProductsController : BasicHomeController
    {
        public ProductsController(IGenericRepository<Product> productRepository, IGenericRepository<Category> categoryRepository) : base(productRepository, categoryRepository)
        {
            
        }

        
    }
}
