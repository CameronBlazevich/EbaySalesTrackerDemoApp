namespace EbaySalesTracker.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class someChanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Listings", "CurrentPrice", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Listings", "CurrentPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
