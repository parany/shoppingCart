using ShoppingCart.BackOffice.ViewsModels;
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
    public class ProductController : Controller
    {
        private IGenericRepository<Product> ProductRepository { get; }
        private IGenericRepository<Category> CategoryRepository { get; }
        private IGenericRepository<FilePath> FilePathRepository { get; }

        public ProductController(IGenericRepository<Product> productRepository, IGenericRepository<Category> categoryRepository, IGenericRepository<FilePath> filePathRepository)
        {
            ProductRepository = productRepository;
            CategoryRepository = categoryRepository;
            FilePathRepository = filePathRepository;
            ProductRepository.AddNavigationProperty(p => p.Category);
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
            CreateViewModels createVM = new CreateViewModels
            {
                CategoryList = new SelectList(CategoryRepository.GetAll(), "Id", "Name")
            };
            return View(createVM);
        }

        // POST: Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateViewModels createViewModels, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                createViewModels.Product.Id = Guid.NewGuid();
                /*if (upload != null && upload.ContentLength > 0)
                {
                    FilePath photo = new FilePath
                    {
                        Id = Guid.NewGuid(),
                        FileName = System.IO.Path.GetFileName(upload.FileName),
                        ProductId = createViewModels.Product.Id
                    };
                    FilePathRepository.Add(photo);
                }*/
                
                ProductRepository.Add(createViewModels.Product);
                return RedirectToAction("Index");
            }

            createViewModels.CategoryList = new SelectList(CategoryRepository.GetAll(), "Id", "Name");

            return View(createViewModels);
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

            CreateViewModels createVM = new CreateViewModels
            {
                Product = product,
                CategoryList = new SelectList(CategoryRepository.GetAll(), "Id", "Name")
            };
            return View(createVM);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CreateViewModels createViewModels)
        {
            if (ModelState.IsValid)
            {
                ProductRepository.Update(createViewModels.Product);
                return RedirectToAction("Index");
            }
            createViewModels.CategoryList = new SelectList(CategoryRepository.GetAll(), "Id", "Name");
            return View(createViewModels);
        }

        // GET: Product/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = ProductRepository.GetSingle(p => p.Id == id);
            ProductRepository.Delete(product);
            return RedirectToAction("Index");
        }
    }
}