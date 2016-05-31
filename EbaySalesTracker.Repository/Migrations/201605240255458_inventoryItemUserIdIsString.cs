namespace EbaySalesTracker.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inventoryItemUserIdIsString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.InventoryItems", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InventoryItems", "UserId", c => c.Int(nullable: false));
        }
    }
}
