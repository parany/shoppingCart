using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Linq;

using ShoppingCart.BackOffice.ViewsModels;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;
using ShoppingCart.Models.Repositories.Concrete;

namespace ShoppingCart.BackOffice.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ProductController : Controller
    {
        private ProductRepository ProductRepository { get; }
        private IGenericRepository<Category> CategoryRepository { get; }
        private IGenericRepository<Image> ImageRepository { get; }
        private IGenericRepository<Provider> ProviderRepository { get; set; }

        public ProductController(ProductRepository productRepository,
                                 IGenericRepository<Category> categoryRepository,
                                 IGenericRepository<Image> imageRepository,
                                 IGenericRepository<Provider> providerRepository)
        {
            ProductRepository = productRepository;
            CategoryRepository = categoryRepository;
            ImageRepository = imageRepository;
            ProviderRepository = providerRepository;
            ProductRepository.AddNavigationProperty(p => p.Category);
            ProductRepository.AddNavigationProperties(p => p.Providers);
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
            var providersList = new List<SelectListItem>();

            var allProviders = ProviderRepository.GetAll();

            foreach (var provider in allProviders)
            {
                providersList.Add(new SelectListItem
                {
                    Value = provider.Id.ToString(),
                    Text = provider.Name
                });
            }

            ViewBag.ProvidersList = providersList;

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

                UploadImage(createViewModels, upload);
                var providers = new List<Provider>();
                if (createViewModels.Providers != null)
                {
                    foreach (var provider in createViewModels.Providers)
                    {
                        var p = ProviderRepository.GetSingle(x => x.Id.Equals(new Guid(provider)));
                        if (p.Products == null)
                            p.Products = new List<Product>();
                        p.Products.Add(createViewModels.Product);
                        providers.Add(p);
                    }
                }
                createViewModels.Product.Providers = providers;
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
                CategoryList = new SelectList(CategoryRepository.GetAll(), "Id", "Name"),
                Providers = product.Providers.Select(x => x.Id.ToString()).ToArray()
            };
            var providersList = new List<SelectListItem>();

            var allProviders = ProviderRepository.GetAll();

            foreach (var provider in allProviders)
            {
                providersList.Add(new SelectListItem
                {
                    Value = provider.Id.ToString(),
                    Text = provider.Name,
                    Selected = createVM.Providers.Any(provider.Id.ToString().Contains)
                });
            }

            ViewBag.ProvidersList = providersList;

            return View(createVM);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CreateViewModels createViewModels, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                UploadImage(createViewModels, upload);
                var providers = new List<Provider>();
                if (createViewModels.Providers != null)
                {
                    foreach (var provider in createViewModels.Providers)
                    {
                        var p = ProviderRepository.GetSingle(x => x.Id.Equals(new Guid(provider)));
                        if (p.Products == null)
                            p.Products = new List<Product>();
                        p.Products.Add(createViewModels.Product);
                        providers.Add(p);
                    }
                }
                createViewModels.Product.Providers = providers;
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

        private void UploadImage(CreateViewModels cvm, HttpPostedFileBase upload)
        {
            if (upload != null && upload.ContentLength > 0)
            {
                var path = Server.MapPath("~/Uploads/images/");
                String imageName = "product_" + cvm.Product.Id.ToString();

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
                cvm.Product.ImageId = uploadImage.Id;
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
                cvm.Product.ImageId = img_default.Id;
            }
        }
    }
}