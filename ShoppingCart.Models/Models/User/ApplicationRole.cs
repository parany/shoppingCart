using Microsoft.AspNet.Identity.EntityFramework;

namespace ShoppingCart.Models.Models.User
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string name) : base(name) { }

        public string Description { get; set; }
    }
}
