namespace EbaySalesTracker.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTransactionsTable : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ListingTransactions", "ListingId", "dbo.Listings");
            DropIndex("dbo.ListingTransactions", new[] { "ListingId" });
            DropTable("dbo.ListingTransactions");
        }
    }
}
