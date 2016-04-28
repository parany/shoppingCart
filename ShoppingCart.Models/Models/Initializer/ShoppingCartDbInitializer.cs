using System;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Models.User;
using System.Collections.Generic;
using System.Linq;

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
            // Products Informations
            string[] categories = { "Ordinateurs", "Tablettes", "Phones" };

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

            // Informations about Identity
            string[] _initialGroupNames = { "Administrator", "Provider", "PrivilegedUser" };

            string[] _rolesNames = { "AllPermissions", "ReadWrite", "Read" };
            string[] _rolesDescriptions = { "These are the administrators who have all access in the system", "These are the providers who supply products to the system", "These are users with privileged access" };

            string[] _usersNames = { "Admin", "Provider", "User" };
            string[] _usersEmails = { "admin@example.com", "provider@example.com", "user@example.com" };
            string[] _usersAddresses = { "Ankorondrano, Madagascar", "Ny Havana, Ankorondrano", "Village des jeux, Ankorondrano" };
            string[] _usersPhoneNumbers = { "0202002020", "0343403434", "0323203232" };
            bool _emailConfirmed = true;
            string _usersPassword = "Secret$1234";

            string[] _administratorRoleNames =  { "AllPermissions" };
            string[] _providerRoleNames = { "ReadWrite" };
            string[] _privilegedUserRoleNames = { "Read" };


            // Adding identity manager
            IdentityManager _idManager = new IdentityManager(context);
            ShoppingCartDbContext _db = context;

            // Adding Groups
            foreach (var groupName in _initialGroupNames)
            {
                _idManager.CreateGroup(groupName);
            }

            // Adding Roles
            for (int j=0; j< _rolesNames.Count(); j++)
            {
                _idManager.CreateRole(_rolesNames[j],_rolesDescriptions[j]);
            }
            

            //Creating Users
            for (int i = 0; i < _usersNames.Count(); i++)
            {
                var user = new ApplicationUser()
                {
                    UserName = _usersNames[i],
                    Email = _usersEmails[i],
                    Address = _usersAddresses[i],
                    EmailConfirmed = _emailConfirmed,
                    PhoneNumber = _usersPhoneNumbers[i]
                };
                _idManager.CreateUser(user, _usersPassword);

                if (i == 0)
                {
                    _idManager.AddUserToRole(user.Id, _rolesNames[0]);
                }
                else if (i == 1)
                {
                    _idManager.AddUserToRole(user.Id, _rolesNames[1]);
                }
            }

            // Adding Roles to Groups
            var allGroups = _db.Groups;
            for(int k=0; k < _initialGroupNames.Count(); k++)
            {
                string[] table = null;
                if (k == 0)
                {
                    table = _administratorRoleNames;
                }
                else if (k == 1)
                {
                    table = _providerRoleNames;
                }
                else if (k == 2)
                {
                    table = _privilegedUserRoleNames;
                }
                string temp = _initialGroupNames[k];
                var roles = allGroups.First(g => g.Name == temp);
                foreach (string name in table)
                {
                    _idManager.AddRoleToGroup(roles.Id, name);
                }
            }

            // Adding Users to Groups
            for(int l = 0; l < _usersNames.Count(); l++)
            {
                string temp = _usersNames[l];
                string temp2 = _initialGroupNames[l];
                var usr = _db.Users.First(u => u.UserName == temp);
                var allGroup = _db.Groups;
                var grp = allGroups.First(g => g.Name == temp2);
                _idManager.AddUserToGroup(usr.Id, grp.Id);
            }
        }
    }
}
