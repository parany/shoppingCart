using System;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Models.User;
using System.Collections.Generic;

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
            Category cat1 = new Category() { Id = Guid.NewGuid(), Name = "Ordinateurs", DateCreated = DateTime.Now };
            context.Categories.Add(cat1);
            Category cat2 = new Category() { Id = Guid.NewGuid(), Name = "Tablettes", DateCreated = DateTime.Now };
            context.Categories.Add(cat2);
            Category cat3 = new Category() { Id = Guid.NewGuid(), Name = "Phones", DateCreated = DateTime.Now };
            context.Categories.Add(cat3);

            // Creating a default image for products
            Image img_default = new Image() { Id = Guid.NewGuid(), ImageName = "product_default", ImageType = ".jpg" };
            context.Images.Add(img_default);

            //Adding 6 Providers
            Provider provd1 = new Provider() { Id = Guid.NewGuid(), Name = "Us Cellular", PaymentMethods = "0", DateCreated = DateTime.Now };
            Provider provd2 = new Provider() { Id = Guid.NewGuid(), Name = "Verizon", PaymentMethods = "0,1", DateCreated = DateTime.Now };
            Provider provd3 = new Provider() { Id = Guid.NewGuid(), Name = "iinet", PaymentMethods = "1,2", DateCreated = DateTime.Now };
            Provider provd4 = new Provider() { Id = Guid.NewGuid(), Name = "Internode", PaymentMethods = "0,1,2", DateCreated = DateTime.Now };
            Provider provd5 = new Provider() { Id = Guid.NewGuid(), Name = "HP Connect", PaymentMethods = "0,2", DateCreated = DateTime.Now };
            Provider provd6 = new Provider() { Id = Guid.NewGuid(), Name = "Hi-Tech System", PaymentMethods = "2", DateCreated = DateTime.Now };
            context.Providers.Add(provd1);
            context.Providers.Add(provd2);
            context.Providers.Add(provd3);
            context.Providers.Add(provd4);
            context.Providers.Add(provd5);
            context.Providers.Add(provd6);

            // Adding 6 products
            Product p1 = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "HP Probook 4540s",
                CategoryId = cat1.Id,
                Providers = new List<Provider>(),
                Description = "Hewlet Packard Laptop",
                ImageId = img_default.Id,
                Price = 700,
                Quantity = 10,
                Type = ProductType.ToBuy,
                ProductReference = Guid.NewGuid(),
                DateCreated = DateTime.Now
            };
            p1.Providers.Add(provd5);
            context.Products.Add(p1);

            Product p2 = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "ASUS R510L",
                CategoryId = cat1.Id,
                Providers = new List<Provider>(),
                Description = "ASUSTek Laptop",
                ImageId = img_default.Id,
                Price = 850,
                Quantity = 10,
                Type = ProductType.ToBuy,
                ProductReference = Guid.NewGuid(),
                DateCreated = DateTime.Now
            };
            p2.Providers.Add(provd6);
            context.Products.Add(p2);

            Product p3 = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "iPad Air 2",
                CategoryId = cat2.Id,
                Providers = new List<Provider>(),
                Description = "Tablet of Apple",
                ImageId = img_default.Id,
                Price = 925,
                Quantity = 10,
                Type = ProductType.ToBuy,
                ProductReference = Guid.NewGuid(),
                DateCreated = DateTime.Now
            };
            p3.Providers.Add(provd4);
            p3.Providers.Add(provd3);
            context.Products.Add(p3);

            Product p4 = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Surface Pro 4",
                CategoryId = cat2.Id,
                Providers = new List<Provider>(),
                Description = "Tablet of Windows",
                ImageId = img_default.Id,
                Price = 950,
                Quantity = 10,
                Type = ProductType.ToBuy,
                ProductReference = Guid.NewGuid(),
                DateCreated = DateTime.Now
            };
            p4.Providers.Add(provd3);
            context.Products.Add(p4);

            Product p5 = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "LG G5",
                CategoryId = cat3.Id,
                Providers = new List<Provider>(),
                Description = "LG smart phone",
                ImageId = img_default.Id,
                Price = 1020,
                Quantity = 10,
                Type = ProductType.ToBuy,
                ProductReference = Guid.NewGuid(),
                DateCreated = DateTime.Now
            };
            p5.Providers.Add(provd1);
            p5.Providers.Add(provd2);
            context.Products.Add(p5);

            Product p6 = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Galaxy S7",
                CategoryId = cat3.Id,
                Providers = new List<Provider>(),
                Description = "Samsung smart phone",
                ImageId = img_default.Id,
                Price = 1250,
                Quantity = 10,
                Type = ProductType.ToBuy,
                ProductReference = Guid.NewGuid(),
                DateCreated = DateTime.Now
            };
            p6.Providers.Add(provd2);
            context.Products.Add(p6); 

            InitializeIdentityForEF(context);

        }

        private void InitializeIdentityForEF(ShoppingCartDbContext context)
        {

            // Adding a default administrator access
            var userMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleMgr = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(context));

            // Informations
            string roleAdminName = "Administrator";
            string roleAdminDesc = "These are the administrators who have all access in the system";
            string roleProviderName = "Provider";
            string roleProviderDesc = "These are the providers who have supply product to the system";
            string userAdminName = "Admin";
            string userAdminPassword = "Secret$1234";
            string userAdminEmail = "admin@example.com";
            string userAdminAddress = "Ankorondrano, Madagascar";
            string userAdminPhoneNumber = "0202002020";
            string userProviderName = "Provider";
            string userProviderPassword = "Secret$1234";
            string userProviderEmail = "provider@example.com";
            string userProviderAddress = "Ny Havana, Ankorondrano";
            string userProviderPhoneNumber = "0343403434";
            string userName = "User";
            string userPassword = "Secret$1234";
            string userEmail = "user@example.com";
            string userAddress = "Village des jeux, Ankorondrano";
            string userPhoneNumber = "0323203232";
            bool emailConfirmed = true;

            // Create Role Admin if it does not exist
            if (!roleMgr.RoleExists(roleAdminName))
            {
                ApplicationRole admin = new ApplicationRole(roleAdminName);
                admin.Description = roleAdminDesc;
                roleMgr.Create(admin);
            }

            // Create Role Provider if it does not exist
            if (!roleMgr.RoleExists(roleProviderName))
            {
                ApplicationRole provider = new ApplicationRole(roleProviderName);
                provider.Description = roleProviderDesc;
                roleMgr.Create(provider);
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

            // Create Provider user
            ApplicationUser user2 = userMgr.FindByName(userProviderName);
            if (user2 == null)
            {
                userMgr.Create(new ApplicationUser
                {
                    UserName = userProviderName,
                    Email = userProviderEmail,
                    EmailConfirmed = emailConfirmed,
                    Address = userProviderAddress,
                    PhoneNumber = userProviderPhoneNumber
                },
                userProviderPassword);
                user2 = userMgr.FindByName(userProviderName);
            }

            // Add User Provider to Role Provider
            if (!userMgr.IsInRole(user2.Id, roleProviderName))
            {
                userMgr.AddToRole(user2.Id, roleProviderName);
            }

            //Create Simple User
            ApplicationUser user3 = userMgr.FindByName(userName);
            if (user3 == null)
            {
                userMgr.Create(new ApplicationUser
                {
                    UserName = userName,
                    Email = userEmail,
                    EmailConfirmed = emailConfirmed,
                    Address = userAddress,
                    PhoneNumber = userPhoneNumber
                },
                userPassword);
                user3 = userMgr.FindByName(userName);
            }
        }
    }
}
