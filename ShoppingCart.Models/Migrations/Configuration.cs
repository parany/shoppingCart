namespace ShoppingCart.Models.Migrations
{
    using ShoppingCart.Models.Models.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web;
    using System.Web.Security;

    internal sealed class Configuration : DbMigrationsConfiguration<ShoppingCart.Models.ShoppingCartDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ShoppingCart.Models.ShoppingCartDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            /*if (!Roles.RoleExists("Administrator"))
            {
                Roles.CreateRole("Administrator");
            }

            if (Membership.GetUser("Admin") == null)
            {
                Membership.CreateUser("Admin", "AdminPass1");
                Roles.AddUserToRole("Admin", "Administrator");
            }*/

        }
    }
}
