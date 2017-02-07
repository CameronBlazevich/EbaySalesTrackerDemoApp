namespace EbaySalesTracker.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrdersDontHaveListingIds : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "ListingId", "dbo.Listings");
            DropIndex("dbo.Orders", new[] { "ListingId" });
            DropColumn("dbo.Orders", "ListingId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "ListingId", c => c.Long(nullable: false));
            CreateIndex("dbo.Orders", "ListingId");
            AddForeignKey("dbo.Orders", "ListingId", "dbo.Listings", "ItemId", cascadeDelete: true);
        }
    }
}
