namespace EbaySalesTracker.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrdersTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.String(nullable: false, maxLength: 128),
                        TotalCost = c.Double(nullable: false),
                        Shipping = c.Double(nullable: false),
                        Handling = c.Double(nullable: false),
                        SalesPrice = c.Double(nullable: false),
                        PaidTime = c.DateTime(nullable: false),
                        ShippedTime = c.DateTime(nullable: false),
                        TotalTaxAmount = c.Double(nullable: false),
                        OrderStatus = c.Int(nullable: false),
                        ListingId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.Listings", t => t.ListingId, cascadeDelete: true)
                .Index(t => t.ListingId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "ListingId", "dbo.Listings");
            DropIndex("dbo.Orders", new[] { "ListingId" });
            DropTable("dbo.Orders");
        }
    }
}
