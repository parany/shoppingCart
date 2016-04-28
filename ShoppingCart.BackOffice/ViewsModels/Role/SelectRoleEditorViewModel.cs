using Microsoft.AspNet.Identity.EntityFramework;
using ShoppingCart.Models;
using ShoppingCart.Models.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoppingCart.BackOffice.ViewsModels
{
    public class SelectRoleEditorViewModel
    {
        public SelectRoleEditorViewModel() { }
        public SelectRoleEditorViewModel(ApplicationRole role)
        {
            this.RoleName = role.Name;
            this.Description = role.Description;
        }

        public bool Selected { get; set; }

        [Required]
        public string RoleName { get; set; }
        public string Description { get; set; }
    }

}