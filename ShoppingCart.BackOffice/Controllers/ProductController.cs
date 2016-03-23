using ImageResizer;
using ShoppingCart.BackOffice.ViewsModels;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.IO;
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
        private IGenericRepository<Image> ImageRepository { get; }

        public ProductController(IGenericRepository<Product> productRepository, IGenericRepository<Category> categoryRepository, IGenericRepository<Image> imageRepository)
        {
            ProductRepository = productRepository;
            CategoryRepository = categoryRepository;
            ImageRepository = imageRepository;
            ProductRepository.AddNavigationProperty(p => p.Category);
            ProductRepository.AddNavigationProperty(img => img.Image);
        }

        //
        // GET: /Product/
        public ActionResult Index()
        {
            IList<ProductViewModel> products = new List<ProductViewModel>();

            foreach (Product product in ProductRepository.GetAll())
            {
                products.Add(new ProductViewModel
                {
                    Product = product,
                    ImagePath = product.Image.getImageVersion("_thumbnail")
                });
            }

            return View(products);
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
            ProductViewModel productVM = new ProductViewModel
            {
                Product = product,
                ImagePath = product.Image.getImageVersion("_medium")
            };
            return View(productVM);
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
                
                if (upload != null && upload.ContentLength > 0)
                {
                    var path = Server.MapPath("~/Uploads/images/");
                    String imageName = "product_" + createViewModels.Product.Id.ToString();

                    Image uploadImage = new Image
                    {
                        Id = Guid.NewGuid(),
                        ImageName = imageName,
                        ImageType = ".jpg"
                    };

                    uploadImage.ImageName = imageName;
                    uploadImage.ImageType = Path.GetExtension(upload.FileName);

                    //Define the versions to generate
                    uploadImage.Versions.Add("_thumbnail", "maxwidth=100&maxheight=100&format=jpg");
                    uploadImage.Versions.Add("_medium", "maxwidth=700&maxheight=700&format=jpg");
                    uploadImage.Versions.Add("_large", "maxwidth=1200&maxheight=1200&format=jpg");

                    uploadImage.SaveAs(path, upload);
                    ImageRepository.Add(uploadImage);
                    createViewModels.Product.ImageId = uploadImage.Id;
                }
                else
                {
                    Image img_default = ImageRepository.GetSingle(i => i.ImageName == "product_default");

                    if (img_default == null)
                    {
                        img_default = new Image
                        {
                            Id = Guid.NewGuid(),
                            ImageName = "product_default",
                            ImageType = ".jpg"
                        };
                        ImageRepository.Add(img_default);
                    }
                    createViewModels.Product.ImageId = img_default.Id;
                }
                
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