namespace EbaySalesTracker.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransDateAndTransStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ListingTransactions", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.ListingTransactions", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ListingTransactions", "Status");
            DropColumn("dbo.ListingTransactions", "CreatedDate");
        }
    }
}
