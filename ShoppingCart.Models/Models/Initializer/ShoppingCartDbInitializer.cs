using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using ShoppingCart.Models.Models.Entities;

namespace ShoppingCart.Models.Models.Initializer
{
    public class ShoppingCartDbInitializer : DropCreateDatabaseIfModelChanges<ShoppingCartDbContext>
    {
        protected override void Seed(ShoppingCartDbContext context)
        {
            PerformInitialSetup(context);
            base.Seed(context);
        }

        public void PerformInitialSetup(ShoppingCartDbContext context)
        {
            // Adding 3 categories
            Category cat1 = new Category() { Id= Guid.NewGuid(), Name = "Ordinateurs", DateCreated = DateTime.Now };
            Category cat2 = new Category() { Id = Guid.NewGuid(), Name = "Tablettes", DateCreated = DateTime.Now };
            Category cat3 = new Category() { Id = Guid.NewGuid(), Name = "Phones", DateCreated = DateTime.Now };
            context.Categories.Add(cat1);
            context.Categories.Add(cat2);
            context.Categories.Add(cat3);

            // Creating a default image for products
            Image img_default = new Image() { Id = Guid.NewGuid(), ImageName = "product_default", ImageType = ".jpg" };
            context.Images.Add(img_default);

            // Adding 6 products
            Product p1 = new Product() { Id = Guid.NewGuid(), Name ="HP Probook 4540s", CategoryId =cat1.Id, Description ="Hewlet Packard Laptop",
                ImageId = img_default.Id, Price = 150, Quantity = 10, DateCreated = DateTime.Now };
            Product p2 = new Product() { Id = Guid.NewGuid(), Name = "ASUS R510L", CategoryId = cat1.Id, Description = "ASUSTek Laptop",
                ImageId = img_default.Id, Price = 150, Quantity = 10, DateCreated = DateTime.Now };
            Product p3 = new Product(){ Id = Guid.NewGuid(), Name = "iPad Air 2", CategoryId = cat2.Id, Description = "Tablet of Apple",
                ImageId = img_default.Id, Price = 150, Quantity = 10, DateCreated = DateTime.Now };
            Product p4 = new Product() { Id = Guid.NewGuid(), Name = "Surface Pro 4", CategoryId = cat2.Id, Description = "Tablet of Windows",
                ImageId = img_default.Id, Price = 150, Quantity = 10, DateCreated = DateTime.Now };
            Product p5 = new Product() { Id = Guid.NewGuid(), Name = "LG G5", CategoryId = cat3.Id, Description = "LG smart phone",
                ImageId = img_default.Id, Price = 150, Quantity = 10, DateCreated = DateTime.Now };
            Product p6 = new Product() { Id = Guid.NewGuid(), Name = "Galaxy S7", CategoryId = cat3.Id, Description = "Samsung smart phone",
                ImageId = img_default.Id, Price = 150, Quantity = 10, DateCreated = DateTime.Now };
            context.Products.Add(p1);
            context.Products.Add(p2);
            context.Products.Add(p3);
            context.Products.Add(p4);
            context.Products.Add(p5);
            context.Products.Add(p6);
        }
    }
}
