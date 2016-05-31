namespace EbaySalesTracker.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inventoryItemPricesNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.InventoryItems", "AverageSalesPrice", c => c.Double());
            AlterColumn("dbo.InventoryItems", "AverageProfit", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InventoryItems", "AverageProfit", c => c.Double(nullable: false));
            AlterColumn("dbo.InventoryItems", "AverageSalesPrice", c => c.Double(nullable: false));
        }
    }
}
