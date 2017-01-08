namespace EbaySalesTracker.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequireInventoryItemDesc : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.InventoryItems", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InventoryItems", "Description", c => c.String());
        }
    }
}
