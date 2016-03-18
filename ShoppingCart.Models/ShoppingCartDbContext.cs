using ShoppingCart.Models.Models.Entities;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using ShoppingCart.Models.Models.User;

namespace ShoppingCart.Models
{
    public class ShoppingCartDbContext : IdentityDbContext<ApplicationUser>
    {
        public ShoppingCartDbContext()
            : base("name=ShoppingCartDb")
        {
        }

        public static ShoppingCartDbContext Create()
        {
            return new ShoppingCartDbContext();
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartLine> CartLines { get; set; }

        public DbSet<ShippingDetail> ShippingDetails { get; set; }
    }
}
