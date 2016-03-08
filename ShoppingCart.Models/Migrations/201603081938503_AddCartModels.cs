namespace ShoppingCart.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCartModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CartLines",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        CartId = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        DateModified = c.DateTime(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Cart_Id = c.Guid(),
                        Product_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Carts", t => t.Cart_Id)
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .Index(t => t.Cart_Id)
                .Index(t => t.Product_Id);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DateCreated = c.DateTime(),
                        DateModified = c.DateTime(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ShippingDetails",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.String(),
                        CartId = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        DateModified = c.DateTime(),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        Cart_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Carts", t => t.Cart_Id)
                .Index(t => t.Cart_Id);
            
            AddColumn("dbo.Products", "Description", c => c.String());
            AddColumn("dbo.Products", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShippingDetails", "Cart_Id", "dbo.Carts");
            DropForeignKey("dbo.CartLines", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.CartLines", "Cart_Id", "dbo.Carts");
            DropIndex("dbo.ShippingDetails", new[] { "Cart_Id" });
            DropIndex("dbo.CartLines", new[] { "Product_Id" });
            DropIndex("dbo.CartLines", new[] { "Cart_Id" });
            DropColumn("dbo.Products", "Price");
            DropColumn("dbo.Products", "Description");
            DropTable("dbo.ShippingDetails");
            DropTable("dbo.Carts");
            DropTable("dbo.CartLines");
        }
    }
}
