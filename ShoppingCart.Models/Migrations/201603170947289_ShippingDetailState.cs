namespace ShoppingCart.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShippingDetailState : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShippingDetails", "State", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShippingDetails", "State");
        }
    }
}
