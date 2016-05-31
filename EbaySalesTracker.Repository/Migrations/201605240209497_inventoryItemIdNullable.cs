namespace EbaySalesTracker.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inventoryItemIdNullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Listings", "InventoryItemId", "dbo.InventoryItems");
            DropIndex("dbo.Listings", new[] { "InventoryItemId" });
            AlterColumn("dbo.Listings", "InventoryItemId", c => c.Int());
            CreateIndex("dbo.Listings", "InventoryItemId");
            AddForeignKey("dbo.Listings", "InventoryItemId", "dbo.InventoryItems", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Listings", "InventoryItemId", "dbo.InventoryItems");
            DropIndex("dbo.Listings", new[] { "InventoryItemId" });
            AlterColumn("dbo.Listings", "InventoryItemId", c => c.Int(nullable: false));
            CreateIndex("dbo.Listings", "InventoryItemId");
            AddForeignKey("dbo.Listings", "InventoryItemId", "dbo.InventoryItems", "Id", cascadeDelete: true);
        }
    }
}
