namespace EbaySalesTracker.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inventoryItemUserId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InventoryItems", "UserId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InventoryItems", "UserId");
        }
    }
}
