﻿using ShoppingCart.BackOffice.ViewsModels;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.BackOffice.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private IGenericRepository<Product> ProductRepository { get; }
        private IGenericRepository<Category> CategoryRepository { get; }

        public ProductController(IGenericRepository<Product> productRepository, IGenericRepository<Category> categoryRepository)
        {
            ProductRepository = productRepository;
            CategoryRepository = categoryRepository;
        }

        //
        // GET: /Product/
        public ActionResult Index()
        {
            return View(ProductRepository.GetAll());
        }

        // GET: Product/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = ProductRepository.GetSingle(p => p.Id == id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            CreateViewModels CreateVM = new CreateViewModels
            {
                CategoryList = new SelectList(CategoryRepository.GetAll(), "Id", "Name")
            };
            return View(CreateVM);
        }

        // POST: Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateViewModels createVM)
        {
            if (ModelState.IsValid)
            {
                createVM.Product.Id = Guid.NewGuid();
                createVM.Product.CategoryId = createVM.CategoryId;
                ProductRepository.Add(createVM.Product);
                return RedirectToAction("Index");
            }

            createVM.CategoryList = new SelectList(CategoryRepository.GetAll(), "Id", "Name");

            return View(createVM);
        }

        // GET: Product/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = ProductRepository.GetSingle(p => p.Id == id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            CreateViewModels CreateVM = new CreateViewModels
            {
                Product = product,
                CategoryList = new SelectList(CategoryRepository.GetAll(), "Id", "Name")
            };
            return View(CreateVM);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,CategoryId,DateCreated,DateModified,CreatedBy,ModifiedBy")] Product product)
        {
            if (ModelState.IsValid)
            {
                ProductRepository.Update(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Product/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = ProductRepository.GetSingle(p => p.Id == id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Product product = ProductRepository.GetSingle(p => p.Id == id);
            ProductRepository.Delete(product);
            return RedirectToAction("Index");
        }
    }
}