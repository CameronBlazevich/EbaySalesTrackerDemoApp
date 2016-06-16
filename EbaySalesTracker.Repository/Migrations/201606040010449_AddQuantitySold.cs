namespace EbaySalesTracker.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddQuantitySold : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InventoryItems", "QuantitySold", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InventoryItems", "QuantitySold");
        }
    }
}
