namespace ShoppingCart.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CartLines", "Cart_Id", "dbo.Carts");
            DropForeignKey("dbo.CartLines", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.Products", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.ShippingDetails", "Cart_Id", "dbo.Carts");
            DropIndex("dbo.CartLines", new[] { "Cart_Id" });
            DropIndex("dbo.CartLines", new[] { "Product_Id" });
            DropIndex("dbo.Products", new[] { "Category_Id" });
            DropIndex("dbo.ShippingDetails", new[] { "Cart_Id" });
            DropColumn("dbo.CartLines", "CartId");
            DropColumn("dbo.CartLines", "ProductId");
            DropColumn("dbo.Products", "CategoryId");
            DropColumn("dbo.ShippingDetails", "CartId");
            RenameColumn(table: "dbo.CartLines", name: "Cart_Id", newName: "CartId");
            RenameColumn(table: "dbo.CartLines", name: "Product_Id", newName: "ProductId");
            RenameColumn(table: "dbo.Products", name: "Category_Id", newName: "CategoryId");
            RenameColumn(table: "dbo.ShippingDetails", name: "Cart_Id", newName: "CartId");
            AlterColumn("dbo.CartLines", "ProductId", c => c.Guid(nullable: false));
            AlterColumn("dbo.CartLines", "CartId", c => c.Guid(nullable: false));
            AlterColumn("dbo.CartLines", "CartId", c => c.Guid(nullable: false));
            AlterColumn("dbo.CartLines", "ProductId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Products", "CategoryId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Products", "CategoryId", c => c.Guid(nullable: false));
            AlterColumn("dbo.ShippingDetails", "CartId", c => c.Guid(nullable: false));
            AlterColumn("dbo.ShippingDetails", "CartId", c => c.Guid(nullable: false));
            CreateIndex("dbo.CartLines", "ProductId");
            CreateIndex("dbo.CartLines", "CartId");
            CreateIndex("dbo.Products", "CategoryId");
            CreateIndex("dbo.ShippingDetails", "CartId");
            AddForeignKey("dbo.CartLines", "CartId", "dbo.Carts", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CartLines", "ProductId", "dbo.Products", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Products", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ShippingDetails", "CartId", "dbo.Carts", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShippingDetails", "CartId", "dbo.Carts");
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.CartLines", "ProductId", "dbo.Products");
            DropForeignKey("dbo.CartLines", "CartId", "dbo.Carts");
            DropIndex("dbo.ShippingDetails", new[] { "CartId" });
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropIndex("dbo.CartLines", new[] { "CartId" });
            DropIndex("dbo.CartLines", new[] { "ProductId" });
            AlterColumn("dbo.ShippingDetails", "CartId", c => c.Guid());
            AlterColumn("dbo.ShippingDetails", "CartId", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "CategoryId", c => c.Guid());
            AlterColumn("dbo.Products", "CategoryId", c => c.Int(nullable: false));
            AlterColumn("dbo.CartLines", "ProductId", c => c.Guid());
            AlterColumn("dbo.CartLines", "CartId", c => c.Guid());
            AlterColumn("dbo.CartLines", "CartId", c => c.Int(nullable: false));
            AlterColumn("dbo.CartLines", "ProductId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.ShippingDetails", name: "CartId", newName: "Cart_Id");
            RenameColumn(table: "dbo.Products", name: "CategoryId", newName: "Category_Id");
            RenameColumn(table: "dbo.CartLines", name: "ProductId", newName: "Product_Id");
            RenameColumn(table: "dbo.CartLines", name: "CartId", newName: "Cart_Id");
            AddColumn("dbo.ShippingDetails", "CartId", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "CategoryId", c => c.Int(nullable: false));
            AddColumn("dbo.CartLines", "ProductId", c => c.Int(nullable: false));
            AddColumn("dbo.CartLines", "CartId", c => c.Int(nullable: false));
            CreateIndex("dbo.ShippingDetails", "Cart_Id");
            CreateIndex("dbo.Products", "Category_Id");
            CreateIndex("dbo.CartLines", "Product_Id");
            CreateIndex("dbo.CartLines", "Cart_Id");
            AddForeignKey("dbo.ShippingDetails", "Cart_Id", "dbo.Carts", "Id");
            AddForeignKey("dbo.Products", "Category_Id", "dbo.Categories", "Id");
            AddForeignKey("dbo.CartLines", "Product_Id", "dbo.Products", "Id");
            AddForeignKey("dbo.CartLines", "Cart_Id", "dbo.Carts", "Id");
        }
    }
}
