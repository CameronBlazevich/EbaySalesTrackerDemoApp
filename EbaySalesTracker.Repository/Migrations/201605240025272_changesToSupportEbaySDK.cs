namespace EbaySalesTracker.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesToSupportEbaySDK : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ListingDetails", "GrossAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.ListingDetails", "NetAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.Listings", "TotalNetFees", c => c.Double(nullable: false));
            AlterColumn("dbo.Listings", "TotalGrossFees", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Listings", "TotalGrossFees", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Listings", "TotalNetFees", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ListingDetails", "NetAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ListingDetails", "GrossAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
