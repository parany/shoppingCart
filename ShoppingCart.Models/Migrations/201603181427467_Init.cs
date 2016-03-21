namespace ShoppingCart.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ShippingDetails", "CartId", "dbo.Carts");
            DropForeignKey("dbo.ShippingDetails", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.ShippingDetails", new[] { "UserId" });
            DropIndex("dbo.ShippingDetails", new[] { "CartId" });
            AddColumn("dbo.Carts", "UserId", c => c.String(maxLength: 128));
            AddColumn("dbo.Carts", "ShoppingDetailId", c => c.Guid(nullable: false));
            AddColumn("dbo.Carts", "State", c => c.Int(nullable: false));
            AddColumn("dbo.Carts", "ShippingDetail_Id", c => c.Guid());
            AddColumn("dbo.ShippingDetails", "Name", c => c.String());
            AddColumn("dbo.ShippingDetails", "Address", c => c.String());
            AddColumn("dbo.ShippingDetails", "PhoneNumber", c => c.String());
            AlterColumn("dbo.ShippingDetails", "UserId", c => c.String());
            CreateIndex("dbo.Carts", "UserId");
            CreateIndex("dbo.Carts", "ShippingDetail_Id");
            AddForeignKey("dbo.Carts", "ShippingDetail_Id", "dbo.ShippingDetails", "Id");
            AddForeignKey("dbo.Carts", "UserId", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.ShippingDetails", "CartId");
            DropColumn("dbo.ShippingDetails", "State");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ShippingDetails", "State", c => c.Int(nullable: false));
            AddColumn("dbo.ShippingDetails", "CartId", c => c.Guid(nullable: false));
            DropForeignKey("dbo.Carts", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Carts", "ShippingDetail_Id", "dbo.ShippingDetails");
            DropIndex("dbo.Carts", new[] { "ShippingDetail_Id" });
            DropIndex("dbo.Carts", new[] { "UserId" });
            AlterColumn("dbo.ShippingDetails", "UserId", c => c.String(maxLength: 128));
            DropColumn("dbo.ShippingDetails", "PhoneNumber");
            DropColumn("dbo.ShippingDetails", "Address");
            DropColumn("dbo.ShippingDetails", "Name");
            DropColumn("dbo.Carts", "ShippingDetail_Id");
            DropColumn("dbo.Carts", "State");
            DropColumn("dbo.Carts", "ShoppingDetailId");
            DropColumn("dbo.Carts", "UserId");
            CreateIndex("dbo.ShippingDetails", "CartId");
            CreateIndex("dbo.ShippingDetails", "UserId");
            AddForeignKey("dbo.ShippingDetails", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.ShippingDetails", "CartId", "dbo.Carts", "Id", cascadeDelete: true);
        }
    }
}
