using System;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Models.User;
using System.Linq;

namespace ShoppingCart.Models.Models.Initializer
{
    public class ShoppingCartDbInitializer : DropCreateDatabaseAlways<ShoppingCartDbContext>
    {

        protected override void Seed(ShoppingCartDbContext context)
        {
            PerformInitialSetup(context);
            base.Seed(context);
        }

        private void PerformInitialSetup(ShoppingCartDbContext context)
        {
            InitializeProductsForEF(context);
            InitializeIdentityForEF(context);
        }

        // Context and manager
        ShoppingCartDbContext _db;
        GroupManager _gpManager;

        // Informations about products
        string[] _categoriesNames = { "Computers", "Tablets", "Phones" };
        string[] _productsNames = { "HP Probook 4540s", "ASUS R510L", "iPad Air 2", "Surface Pro 4", "LG G5", "Galaxy S7" };
        string[] _productsDescriptions = { "Hewlet Packard Laptop", "ASUSTek Laptop", "Tablet of Apple", "Tablet of Windows", "LG smart phone", "Samsung smart phone" };

        // Informations about Identity
        string[] _initialGroupNames = new string[] { "AllPermissions", "Read/Write", "Read" };
        string[] _rolesNames = { "Administrator", "Provider", "PrivilegedUser" };
        string[] _rolesDescriptions = { "These are the administrators who have all access in the system", "These are the providers who supply products to the system", "These are users with privileged access" };
        string[] _usersNames = { "Admin", "Provider", "User" };
        string _usersPassword = "Secret$1234";
        string[] _usersEmails = { "admin@example.com", "provider@example.com", "user@example.com" };
        string[] _usersAddresses = { "Ankorondrano, Madagascar", "Ny Havana, Ankorondrano", "Village des jeux, Ankorondrano" };
        string[] _usersPhoneNumbers = { "0202002020", "0343403434", "0323203232" };
        bool emailConfirmed = true;
        string[] _allPermissionsRoleNames = new string[] { "Admin" };
        string[] _readWriteRoleNames = new string[] { "Admin", "Provider" };
        string[] _readRoleNames = new string[] { "Admin", "PrivilegedUser" };

        private void InitializeProductsForEF(ShoppingCartDbContext context)
        {


            // Creating a default image for products
            Image img_default = new Image() { Id = Guid.NewGuid(), ImageName = "product_default", ImageType = ".jpg" };
            context.Images.Add(img_default);

            // Adding products
            int k = -1;
            Category cat = null;
            for (int j = 0; j < _productsNames.Count(); j++)
            {
                if (j == 0 || j % 2 == 0)
                {
                    k++;
                    cat = new Category() { Id = Guid.NewGuid(), Name = _categoriesNames[k], DateCreated = DateTime.Now };
                    context.Categories.Add(cat);
                }

                Product p = new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = _productsNames[j],
                    CategoryId = cat.Id,
                    Description = _productsDescriptions[j],
                    ImageId = img_default.Id,
                    Price = 700,
                    Quantity = 10,
                    DateCreated = DateTime.Now
                };
                context.Products.Add(p);
            }
        }

        private void InitializeIdentityForEF(ShoppingCartDbContext context)
        {

            // Adding user and role manager
            //var userMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            //var roleMgr = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(context));

            //// Create Role Admin if it does not exist
            //if (!roleMgr.RoleExists(roleAdminName))
            //{
            //    ApplicationRole admin = new ApplicationRole(roleAdminName, roleAdminDesc);
            //    roleMgr.Create(admin);
            //}

            //// Create Role Provider if it does not exist
            //if (!roleMgr.RoleExists(roleProviderName))
            //{
            //    ApplicationRole provider = new ApplicationRole(roleProviderName, roleProviderDesc);
            //    roleMgr.Create(provider);
            //}

            //// Create Admin user
            //ApplicationUser user = userMgr.FindByName(userAdminName);
            //if (user == null)
            //{
            //    userMgr.Create(new ApplicationUser
            //    {
            //        UserName = userAdminName,
            //        Email = userAdminEmail,
            //        EmailConfirmed = emailConfirmed,
            //        Address = userAdminAddress,
            //        PhoneNumber = userAdminPhoneNumber
            //    },
            //    userAdminPassword);
            //    user = userMgr.FindByName(userAdminName);
            //}

            //// Add User Admin to Role Admin
            //if (!userMgr.IsInRole(user.Id, roleAdminName))
            //{
            //    userMgr.AddToRole(user.Id, roleAdminName);
            //}

            //// Create Provider user
            //ApplicationUser user2 = userMgr.FindByName(userProviderName);
            //if (user2 == null)
            //{
            //    userMgr.Create(new ApplicationUser
            //    {
            //        UserName = userProviderName,
            //        Email = userProviderEmail,
            //        EmailConfirmed = emailConfirmed,
            //        Address = userProviderAddress,
            //        PhoneNumber = userProviderPhoneNumber
            //    },
            //    userProviderPassword);
            //    user2 = userMgr.FindByName(userProviderName);
            //}

            //// Add User Provider to Role Provider
            //if (!userMgr.IsInRole(user2.Id, roleProviderName))
            //{
            //    userMgr.AddToRole(user2.Id, roleProviderName);
            //}

            ////Create Simple User
            //ApplicationUser user3 = userMgr.FindByName(userName);
            //if (user3 == null)
            //{
            //    userMgr.Create(new ApplicationUser
            //    {
            //        UserName = userName,
            //        Email = userEmail,
            //        EmailConfirmed = emailConfirmed,
            //        Address = userAddress,
            //        PhoneNumber = userPhoneNumber
            //    },
            //    userPassword);
            //    user3 = userMgr.FindByName(userName);
            //}

            _gpManager = new GroupManager(context);
            _db = context;

            this.AddGroups();
            this.AddRoles();
            this.AddUsers();
            this.AddRolesToGroups();
            this.AddUsersToGroups();
        }

        void AddGroups()
        {
            foreach (var groupName in _initialGroupNames)
            {
                _gpManager.CreateGroup(groupName);
            }
        }


        void AddRoles()
        {
            // Some example initial roles.
            for (int i = 0; i < _rolesNames.Count(); i++)
            {
                _gpManager.CreateRole(_rolesNames[i], _rolesDescriptions[i]);
            }
        }

        void AddRolesToGroups()
        {
            // Add the Admin Roles to the Admin Group:
            var allGroups = _db.Groups;
            var allPermissions = allGroups.First(g => g.Name == _initialGroupNames[0]);
            foreach (string name in _allPermissionsRoleNames)
            {
                _gpManager.AddRoleToGroup(allPermissions.Id, name);
            }

            // Add the read/write Roles to the read/write Group:
            var readWrite = _db.Groups.First(g => g.Name == _initialGroupNames[1]);
            foreach (string name in _readWriteRoleNames)
            {
                _gpManager.AddRoleToGroup(readWrite.Id, name);
            }

            // Add the read Roles to the read Group:
            var read = _db.Groups.First(g => g.Name == _initialGroupNames[2]);
            foreach (string name in _readRoleNames)
            {
                _gpManager.AddRoleToGroup(read.Id, name);
            }
        }

        void AddUsers()
        {
            for (int m = 0; m < _usersNames.Count(); m++)
            {
                var newUser = new ApplicationUser()
                {
                    UserName = _usersNames[m],
                    Email = _usersEmails[m],
                    Address = _usersAddresses[m],
                    EmailConfirmed = emailConfirmed,
                    PhoneNumber = _usersPhoneNumbers[m]
                };
                _gpManager.CreateUser(newUser, _usersPassword);
            }

        }


        // Configure the initial Admin user:
        void AddUsersToGroups()
        {
            var admin = _db.Users.First(u => u.UserName == _usersNames[0]);
            var allGroups = _db.Groups;
            foreach (var group in allGroups)
            {
                _gpManager.AddUserToGroup(admin.Id, group.Id);
            }

            for (int n = 1; n < _usersNames.Count(); n++)
            {
                var provider = _db.Users.First(u => u.UserName == _usersNames[n]);
                var readWrite = _db.Groups.First(g => g.Name == _initialGroupNames[n]);
                _gpManager.AddUserToGroup(provider.Id, readWrite.Id);
            }

        }
    }
}
