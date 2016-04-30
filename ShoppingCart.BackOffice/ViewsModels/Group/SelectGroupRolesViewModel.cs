using ShoppingCart.Models;
using ShoppingCart.Models.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.BackOffice.ViewsModels
{
    public class SelectGroupRolesViewModel
    {
        public SelectGroupRolesViewModel()
        {
            this.Roles = new List<SelectRoleEditorViewModel>();
        }


        // Enable initialization with an instance of ApplicationUser:
        public SelectGroupRolesViewModel(Group group)
            : this()
        {
            this.GroupId = group.Id;
            this.GroupName = group.Name;

            var Db = new ShoppingCartDbContext();

            // Add all available roles to the list of EditorViewModels:
            var allRoles = Db.Roles;
            foreach (var role in allRoles)
            {
                // An EditorViewModel will be used by Editor Template:
                var rvm = new SelectRoleEditorViewModel(role);
                this.Roles.Add(rvm);
            }

            // Set the Selected property to true for those roles for 
            // which the current user is a member:
            foreach (var groupRole in group.Roles)
            {
                var checkGroupRole =
                    this.Roles.Find(r => r.RoleName == groupRole.Role.Name);
                checkGroupRole.Selected = true;
            }
        }

        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public List<SelectRoleEditorViewModel> Roles { get; set; }
    }
}