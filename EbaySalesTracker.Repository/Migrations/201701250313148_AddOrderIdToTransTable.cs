namespace EbaySalesTracker.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderIdToTransTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ListingTransactions", "OrderId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ListingTransactions", "OrderId");
        }
    }
}
