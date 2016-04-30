using ShoppingCart.Models;
using ShoppingCart.Models.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.BackOffice.ViewsModels
{
    public class SelectUserGroupsViewModel
    {
        public string UserName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public List<SelectGroupEditorViewModel> Groups { get; set; }

        public SelectUserGroupsViewModel()
        {
            this.Groups = new List<SelectGroupEditorViewModel>();
        }

        public SelectUserGroupsViewModel(ApplicationUser user)
            : this()
        {
            this.UserName = user.UserName;
            this.Address = user.Address;
            this.PhoneNumber = user.PhoneNumber;

            var Db = new ShoppingCartDbContext();

            // Add all available groups to the public list:
            var allGroups = Db.Groups;
            foreach (var role in allGroups)
            {
                // An EditorViewModel will be used by Editor Template:
                var rvm = new SelectGroupEditorViewModel(role);
                this.Groups.Add(rvm);
            }

            // Set the Selected property to true where user is already a member:
            foreach (var group in user.Groups)
            {
                var checkUserRole =
                    this.Groups.Find(r => r.GroupName == group.Group.Name);
                checkUserRole.Selected = true;
            }
        }
    }
}