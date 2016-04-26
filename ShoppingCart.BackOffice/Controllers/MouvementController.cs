using Microsoft.CSharp.RuntimeBinder;
using ShoppingCart.BackOffice.ViewsModels;
using ShoppingCart.Models.Log;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Concrete;
using ShoppingCart.Models.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.BackOffice.Controllers
{
    public class MouvementController : Controller
    {
        private IGenericRepository<ChangeTracking> _ChangeTrackingRepository { get; }
        private ProductRepository _ProductRepository { get; }

        public MouvementController(IGenericRepository<ChangeTracking> changeTrackingRepository, ProductRepository productRepository)
        {
            _ChangeTrackingRepository = changeTrackingRepository;
            _ProductRepository = productRepository;
        }

        // GET: Mouvement
        public ActionResult Index()
        {
            return View(_ChangeTrackingRepository.GetAll().OrderByDescending(mvt => mvt.DateChanged));
        }

        // GET: Mouvement/Stock
        public ActionResult Stock(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChangeTracking change = _ChangeTrackingRepository.GetSingle(mvt => mvt.Id == id.Value
                                                                               && mvt.ClassName == "Product");
            IList<Product> products = _ProductRepository.GetAll();

            IList<StockHistoryViewModel> stockHistoryList = new List<StockHistoryViewModel>();

            foreach (Product pro in products)
            {
                stockHistoryList.Add(new StockHistoryViewModel { Product = pro });
            }

            StockHistoryViewModel productStockChanged = stockHistoryList.FirstOrDefault(p => p.Product.Id == change.PrimaryKey);

            if (productStockChanged != null)
            {

                var property = productStockChanged.Product.GetType().GetProperty(change.PropertyName);
                property.SetValue(productStockChanged.Product, change.OldValue,null,Binder.);
            }

            return View(productStockChanged);
        }
    }
}