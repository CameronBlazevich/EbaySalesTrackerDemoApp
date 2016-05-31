namespace EbaySalesTracker.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inventoryItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InventoryItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Cost = c.Double(nullable: false),
                        Quantity = c.Double(nullable: false),
                        AverageSalesPrice = c.Double(nullable: false),
                        AverageProfit = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Listings", "InventoryItemId", c => c.Int(nullable: true));
            CreateIndex("dbo.Listings", "InventoryItemId");
            AddForeignKey("dbo.Listings", "InventoryItemId", "dbo.InventoryItems", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Listings", "InventoryItemId", "dbo.InventoryItems");
            DropIndex("dbo.Listings", new[] { "InventoryItemId" });
            DropColumn("dbo.Listings", "InventoryItemId");
            DropTable("dbo.InventoryItems");
        }
    }
}
