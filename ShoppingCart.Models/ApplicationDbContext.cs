using Microsoft.AspNet.Identity.EntityFramework;
using ShoppingCart.Models.Models.User;

namespace ShoppingCart.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("UserDb", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}