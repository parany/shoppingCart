﻿using Microsoft.CSharp.RuntimeBinder;
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
        private IGenericRepository<Category> _CategoryRepository { get; }
        private IGenericRepository<Image> _ImageRepository { get; }
        private IGenericRepository<Provider> _ProviderRepository { get; set; }

        public MouvementController(IGenericRepository<ChangeTracking> changeTrackingRepository,
                                   ProductRepository productRepository,
                                   IGenericRepository<Category> categoryRepository,
                                   IGenericRepository<Image> imageRepository,
                                   IGenericRepository<Provider> providerRepository)
        {
            _ChangeTrackingRepository = changeTrackingRepository;
            _ProductRepository = productRepository;
            _CategoryRepository = categoryRepository;
            _ImageRepository = imageRepository;
            _ProviderRepository = providerRepository;
            _ProductRepository.AddNavigationProperty(p => p.Category);
            _ProductRepository.AddNavigationProperty(img => img.Image);
            _ProductRepository.AddNavigationProperties(p => p.Providers);
        }

        // GET: Mouvement
        [Authorize(Roles = "AllPermissions")]
        public ActionResult Index()
        {
            return View(_ChangeTrackingRepository.GetAll().OrderByDescending(mvt => mvt.DateChanged));
        }

        // GET: Mouvement
        public ActionResult Avant(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ChangeTracking change = _ChangeTrackingRepository.GetSingle(mvt => mvt.Id == id.Value
                                                                               && mvt.ClassName == "Product");
            IList<Product> products = _ProductRepository.GetList(p => p.Type == ProductType.ForSale
                                                                      && p.DateCreated <= change.DateChanged);

            IList<ChangeTracking> list = _ChangeTrackingRepository.GetList(mvt => mvt.DateChanged > change.DateChanged 
                                                                                  && mvt.Type != ChangeType.Delete);

            IList<StockHistoryViewModel> stockHistoryList = new List<StockHistoryViewModel>();

            foreach (Product pro in products)
            {
                stockHistoryList.Add(new StockHistoryViewModel { Product = pro, ImagePath = pro.Image.getImageVersion("_thumbnail"), ChangedType = null });
            }

            foreach (ChangeTracking chg in list)
            {
                switch (chg.Type)
                {
                    case ChangeType.Add:
                        break;
                    case ChangeType.Edit:
                        StockHistoryViewModel productStockChanged = stockHistoryList.FirstOrDefault(p => p.Product.Id == chg.PrimaryKey);
                        if (productStockChanged != null)
                        {
                            var property = productStockChanged.Product.GetType().GetProperty(chg.PropertyName);
                            var targetType = property.PropertyType;
                            var convertedValue = Convert.ChangeType(chg.OldValue, targetType);

                            property.SetValue(productStockChanged.Product, convertedValue);
                            productStockChanged.ChangedType = chg.Type.ToString();
                        }
                        break;

                    case ChangeType.Delete:
                        break;
                }
            }

            return View("Stock", stockHistoryList);
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
                stockHistoryList.Add(new StockHistoryViewModel { Product = pro, ImagePath = pro.Image.getImageVersion("_thumbnail"), ChangedType = null });
            }

            StockHistoryViewModel productStockChanged = stockHistoryList.FirstOrDefault(p => p.Product.Id == change.PrimaryKey);

            if (productStockChanged != null)
            {

                var property = productStockChanged.Product.GetType().GetProperty(change.PropertyName);
                var targetType = property.PropertyType;
                var convertedValue = Convert.ChangeType(change.OldValue, targetType);

                property.SetValue(productStockChanged.Product, convertedValue);
                productStockChanged.ChangedType = change.Type.ToString();
            }

            if (change.Type == ChangeType.Delete)
            {
                stockHistoryList.Add(new StockHistoryViewModel() { ChangedType = change.Type.ToString() });
            }
            return View(stockHistoryList);
        }
    }
}