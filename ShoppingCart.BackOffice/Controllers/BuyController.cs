using ShoppingCart.BackOffice.ViewsModels.Buy;
using ShoppingCart.CommonController.Controllers;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.BackOffice.Controllers
{
    public class BuyController : BasicHomeController
    {
        public BuyController(IGenericRepository<Product> productRepository, IGenericRepository<Category> categoryRepository) : base(productRepository, categoryRepository)
        {

        }

        public override ActionResult Index()
        {
            IList<Product> productsToBuy = _ProductRepository.GetList(p => p.Type == ProductType.ToBuy);
            IList<Product> productsInStock = _ProductRepository.GetList(p => p.Type == ProductType.ForSale);
            IList<Category> categories = _CategoryRepository.GetAll();

            IList<ProductStateViewModel> productStateList = new List<ProductStateViewModel>();

            foreach (Product product in productsToBuy)
            {
                ProductStateViewModel productStateVm = new ProductStateViewModel { Product = product };
                productStateVm.ChangeProductState(productsInStock);
                productStateList.Add(productStateVm);
            }

            ProductListAndStateViewModel viewModel = new ProductListAndStateViewModel
            {
                ProductAndState = productStateList.OrderByDescending(p => p.Product.DateCreated),
                Categories = categories
            };

            return View(viewModel);
        }
    }
}