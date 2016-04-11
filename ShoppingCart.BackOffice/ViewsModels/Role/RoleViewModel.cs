using ShoppingCart.Models.Models.User;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.BackOffice.ViewsModels
{
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
}