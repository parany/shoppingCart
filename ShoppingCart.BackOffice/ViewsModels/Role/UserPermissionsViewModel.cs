using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ShoppingCart.Models;
using ShoppingCart.Models.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.BackOffice.ViewsModels
{
    public class UserPermissionsViewModel
    {
        UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ShoppingCartDbContext()));
        RoleManager<ApplicationRole> roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(new ShoppingCartDbContext()));

        public UserPermissionsViewModel()
        {
            this.Roles = new List<RoleViewModel>();
        }


        // Enable initialization with an instance of ApplicationUser:
        public UserPermissionsViewModel(ApplicationUser user)
            : this()
        {
            this.UserName = user.UserName;
            this.Address = user.Address;
            this.PhoneNumber = user.PhoneNumber;
            var roles = userManager.GetRoles(user.Id);

            foreach (var role in roles)
            {
                var currentrole = roleManager.FindByName(role);
                var pvm = new RoleViewModel(currentrole);
                this.Roles.Add(pvm);
            }
        }

        public string UserName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public List<RoleViewModel> Roles { get; set; }
    }
}