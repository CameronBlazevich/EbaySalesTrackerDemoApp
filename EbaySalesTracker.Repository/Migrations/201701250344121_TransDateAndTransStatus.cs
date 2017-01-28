namespace EbaySalesTracker.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransDateAndTransStatus : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ListingTransactions", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ListingTransactions", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ListingTransactions", "Status");
            DropColumn("dbo.ListingTransactions", "CreatedDate");
        }
    }
}
