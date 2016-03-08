using ShoppingCart.Models.Models.Entities;
using System.Data.Entity;

namespace ShoppingCart.Models
{
    public class ShoppingCartDbContext : DbContext
    {
        public ShoppingCartDbContext() : base("name=ShoppingCartDb")
        {
        }

        public DbSet<Product> Products { get; set; }
        
        public DbSet<Category> Categories { get; set; } 

        public DbSet<Cart> Carts { get; set; }
        
        public DbSet<CartLine> CartLines { get; set; } 

        public DbSet<ShippingDetail> ShippingDetails { get; set; } 
    }
}
