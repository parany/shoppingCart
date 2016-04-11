using System;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Models.User;

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
            context.Categories.Add(cat1);
            Category cat2 = new Category() { Id = Guid.NewGuid(), Name = "Tablettes", DateCreated = DateTime.Now };
            context.Categories.Add(cat2);
            Category cat3 = new Category() { Id = Guid.NewGuid(), Name = "Phones", DateCreated = DateTime.Now };
            context.Categories.Add(cat3);

            // Creating a default image for products
            Image img_default = new Image() { Id = Guid.NewGuid(), ImageName = "product_default", ImageType = ".jpg" };
            context.Images.Add(img_default);

            // Adding 6 products
            Product p1 = new Product() { Id = Guid.NewGuid(), Name ="HP Probook 4540s", CategoryId =cat1.Id, Description ="Hewlet Packard Laptop",
                ImageId = img_default.Id, Price = 700, Quantity = 10, DateCreated = DateTime.Now };
            context.Products.Add(p1);
            Product p2 = new Product() { Id = Guid.NewGuid(), Name = "ASUS R510L", CategoryId = cat1.Id, Description = "ASUSTek Laptop",
                ImageId = img_default.Id, Price = 850, Quantity = 10, DateCreated = DateTime.Now };
            context.Products.Add(p2);
            Product p3 = new Product(){ Id = Guid.NewGuid(), Name = "iPad Air 2", CategoryId = cat2.Id, Description = "Tablet of Apple",
                ImageId = img_default.Id, Price = 925, Quantity = 10, DateCreated = DateTime.Now };
            context.Products.Add(p3);
            Product p4 = new Product() { Id = Guid.NewGuid(), Name = "Surface Pro 4", CategoryId = cat2.Id, Description = "Tablet of Windows",
                ImageId = img_default.Id, Price = 950, Quantity = 10, DateCreated = DateTime.Now };
            context.Products.Add(p4);
            Product p5 = new Product() { Id = Guid.NewGuid(), Name = "LG G5", CategoryId = cat3.Id, Description = "LG smart phone",
                ImageId = img_default.Id, Price = 1020, Quantity = 10, DateCreated = DateTime.Now };
            context.Products.Add(p5);
            Product p6 = new Product() { Id = Guid.NewGuid(), Name = "Galaxy S7", CategoryId = cat3.Id, Description = "Samsung smart phone",
                ImageId = img_default.Id, Price = 1250, Quantity = 10, DateCreated = DateTime.Now };
            context.Products.Add(p6);

            InitializeIdentityForEF(context);

        }

        private void InitializeIdentityForEF(ShoppingCartDbContext context)
        {

            // Adding a default administrator access
            var userMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleMgr = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(context));

            // Informations
            string roleAdminName = "Admin";
            string userAdminName = "Admin";
            string userAdminPassword = "Secret$1234";
            bool emailConfirmed = true;
            string userAdminEmail = "admin@example.com";
            string userAdminAddress = "Ankorondrano, Madagascar";
            string userAdminPhoneNumber = "0202002020";

            // Create Role Admin if it does not exist
            if (!roleMgr.RoleExists(roleAdminName))
            {
                roleMgr.Create(new ApplicationRole(roleAdminName));
            }

            // Create Admin user
            ApplicationUser user = userMgr.FindByName(userAdminName);
            if (user == null)
            {
                userMgr.Create(new ApplicationUser
                {
                    UserName = userAdminName,
                    Email = userAdminEmail,
                    EmailConfirmed = emailConfirmed,
                    Address = userAdminAddress,
                    PhoneNumber = userAdminPhoneNumber
                },
                userAdminPassword);
                user = userMgr.FindByName(userAdminName);
            }

            // Add User Admin to Role Admin
            if (!userMgr.IsInRole(user.Id, roleAdminName))
            {
                userMgr.AddToRole(user.Id, roleAdminName);
            }
        }
    }
}
