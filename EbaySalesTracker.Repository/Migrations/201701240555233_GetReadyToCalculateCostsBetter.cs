namespace EbaySalesTracker.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GetReadyToCalculateCostsBetter : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ListingTransactions");
            AddColumn("dbo.ListingDetails", "OrderLineItemId", c => c.String());
            AddColumn("dbo.ListingDetails", "Transactionid", c => c.String());
            AddColumn("dbo.ListingTransactions", "TotalAmountPaid", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ListingTransactions", "UnitPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ListingTransactions", "BuyerShippingCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ListingTransactions", "BuyerHandlingCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ListingTransactions", "QuantitySold", c => c.Int(nullable: false));
            AlterColumn("dbo.ListingTransactions", "TransactionId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.ListingTransactions", "TransactionId");
            DropColumn("dbo.ListingTransactions", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ListingTransactions", "Id", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.ListingTransactions");
            AlterColumn("dbo.ListingTransactions", "TransactionId", c => c.String());
            DropColumn("dbo.ListingTransactions", "QuantitySold");
            DropColumn("dbo.ListingTransactions", "BuyerHandlingCost");
            DropColumn("dbo.ListingTransactions", "BuyerShippingCost");
            DropColumn("dbo.ListingTransactions", "UnitPrice");
            DropColumn("dbo.ListingTransactions", "TotalAmountPaid");
            DropColumn("dbo.ListingDetails", "Transactionid");
            DropColumn("dbo.ListingDetails", "OrderLineItemId");
            AddPrimaryKey("dbo.ListingTransactions", "Id");
        }
    }
}
