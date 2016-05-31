namespace EbaySalesTracker.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        FileId = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        ContentType = c.String(),
                        Content = c.Binary(),
                        FileType = c.Int(nullable: false),
                        HasBeenConsumed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.FileId);
            
            CreateTable(
                "dbo.ListingDetails",
                c => new
                    {
                        RefNumber = c.Long(nullable: false),
                        ItemId = c.Long(nullable: false),
                        Type = c.Int(nullable: false),
                        Description = c.String(),
                        PostDate = c.DateTime(nullable: false),
                        GrossAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NetAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Memo = c.String(),
                    })
                .PrimaryKey(t => t.RefNumber)
                .ForeignKey("dbo.Listings", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.Listings",
                c => new
                    {
                        ItemId = c.Long(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Title = c.String(maxLength: 100),
                        CurrentPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QuantitySold = c.Int(nullable: false),
                        ListingStatus = c.Int(nullable: false),
                        TotalNetFees = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalGrossFees = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserId = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ListingDetails", "ItemId", "dbo.Listings");
            DropIndex("dbo.ListingDetails", new[] { "ItemId" });
            DropTable("dbo.Listings");
            DropTable("dbo.ListingDetails");
            DropTable("dbo.Files");
        }
    }
}
