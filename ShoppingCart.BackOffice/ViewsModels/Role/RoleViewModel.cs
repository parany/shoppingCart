using ShoppingCart.Models.Models.User;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.BackOffice.ViewsModels
{
        public class RoleCreateModel
        {
            [Required]
            public string Name { get; set; }
            public string Description { get; set; }
        }
        public class RoleEditModel
        {
            public ApplicationRole Role { get; set; }
            public IEnumerable<ApplicationUser> Members { get; set; }
            public IEnumerable<ApplicationUser> NonMembers { get; set; }
        }
        public class RoleModificationModel
        {
            [Required]
            public string RoleName { get; set; }
            public string[] IdsToAdd { get; set; }
            public string[] IdsToDelete { get; set; }
        }

        public class RoleIndexModel
        {
            public ApplicationRole Role { get; set; }
            public Collection<ApplicationUser> Users { get; set; }
        }
}