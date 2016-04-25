using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Models.User;
using ShoppingCart.Models.Models.Initializer;


namespace ShoppingCart.Models
{
    public class ShoppingCartDbContext : IdentityDbContext<ApplicationUser>
    {
        public ShoppingCartDbContext()
            : base("name=ShoppingCartDb")
        {
        }

        static ShoppingCartDbContext()
        {
            Database.SetInitializer<ShoppingCartDbContext>(new ShoppingCartDbInitializer());
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

        public DbSet<Provider> Providers { get; set; }

        public virtual DbSet<Group> Groups { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("modelBuilder");
            }
            modelBuilder.Entity<IdentityUser>().ToTable("AspNetUsers");

            EntityTypeConfiguration<ApplicationUser> table =
                modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");

            table.Property((ApplicationUser u) => u.UserName).IsRequired();

            modelBuilder.Entity<ApplicationUser>().HasMany<IdentityUserRole>((ApplicationUser u) => u.Roles);
            modelBuilder.Entity<IdentityUserRole>().HasKey((IdentityUserRole r) =>
                new { UserId = r.UserId, RoleId = r.RoleId }).ToTable("AspNetUserRoles");

            modelBuilder.Entity<ApplicationUser>().HasMany<ApplicationUserGroup>((ApplicationUser u) => u.Groups);
            modelBuilder.Entity<ApplicationUserGroup>().HasKey((ApplicationUserGroup r) =>
                new { UserId = r.UserId, GroupId = r.GroupId }).ToTable("ApplicationUserGroups");

            modelBuilder.Entity<Group>().HasMany<ApplicationRoleGroup>((Group g) => g.Roles);
            modelBuilder.Entity<ApplicationRoleGroup>().HasKey((ApplicationRoleGroup gr) =>
                new { RoleId = gr.RoleId, GroupId = gr.GroupId }).ToTable("ApplicationRoleGroups");

            EntityTypeConfiguration<Group> groupsConfig = modelBuilder.Entity<Group>().ToTable("Groups");
            groupsConfig.Property((Group r) => r.Name).IsRequired();

            EntityTypeConfiguration<IdentityUserLogin> entityTypeConfiguration =
                modelBuilder.Entity<IdentityUserLogin>().HasKey((IdentityUserLogin l) =>
                    new {
                        UserId = l.UserId,
                        LoginProvider = l.LoginProvider,
                        ProviderKey =
                        l.ProviderKey
                    }).ToTable("AspNetUserLogins");

            //entityTypeConfiguration.HasRequired<IdentityUser>((IdentityUserLogin u) => u.User);
            EntityTypeConfiguration<IdentityUserClaim> table1 =
                modelBuilder.Entity<IdentityUserClaim>().ToTable("AspNetUserClaims");

            //table1.HasRequired<IdentityUser>((IdentityUserClaim u) => u.User);

            modelBuilder.Entity<IdentityRole>().ToTable("AspNetRoles");

            EntityTypeConfiguration<ApplicationRole> entityTypeConfiguration1 =
                modelBuilder.Entity<ApplicationRole>().ToTable("AspNetRoles");

            entityTypeConfiguration1.Property((ApplicationRole r) => r.Name).IsRequired();
        }
    }
}
