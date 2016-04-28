using ShoppingCart.Models.Models.User;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.BackOffice.ViewsModels
{
    public class RoleViewModel
    {
        public string RoleName { get; set; }
        public string Description { get; set; }

        public RoleViewModel() { }
        public RoleViewModel(ApplicationRole role)
        {
            this.RoleName = role.Name;
            this.Description = role.Description;
        }
    }
}