namespace EbaySalesTracker.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigForLocalhost : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE dbo.InventoryItems DROP CONSTRAINT DF__Inventory__UserI__286302EC");
            CreateTable(
                "dbo.ListingTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ListingId = c.Long(nullable: false),
                        TransactionId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Listings", t => t.ListingId, cascadeDelete: true)
                .Index(t => t.ListingId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.String(nullable: false, maxLength: 128),
                        TotalCost = c.Double(nullable: false),
                        Shipping = c.Double(nullable: false),
                        Handling = c.Double(nullable: false),
                        SalesPrice = c.Double(nullable: false),
                        PaidTime = c.DateTime(),
                        ShippedTime = c.DateTime(),
                        TotalTaxAmount = c.Double(nullable: false),
                        OrderStatus = c.Int(nullable: false),
                        ListingId = c.Long(nullable: false),
                        RefundAmount = c.Double(),
                        RefundTime = c.DateTime(),
                        RefundStatus = c.Int(),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.Listings", t => t.ListingId, cascadeDelete: true)
                .Index(t => t.ListingId);
            
            AddColumn("dbo.InventoryItems", "QuantitySold", c => c.Int(nullable: false));
            AddColumn("dbo.Listings", "Profit", c => c.Double(nullable: false));
            AddColumn("dbo.Listings", "Type", c => c.Int(nullable: false));
            AlterColumn("dbo.InventoryItems", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "ListingId", "dbo.Listings");
            DropForeignKey("dbo.ListingTransactions", "ListingId", "dbo.Listings");
            DropIndex("dbo.Orders", new[] { "ListingId" });
            DropIndex("dbo.ListingTransactions", new[] { "ListingId" });
            AlterColumn("dbo.InventoryItems", "UserId", c => c.Int(nullable: false));
            DropColumn("dbo.Listings", "Type");
            DropColumn("dbo.Listings", "Profit");
            DropColumn("dbo.InventoryItems", "QuantitySold");
            DropTable("dbo.Orders");
            DropTable("dbo.ListingTransactions");
        }
    }
}
