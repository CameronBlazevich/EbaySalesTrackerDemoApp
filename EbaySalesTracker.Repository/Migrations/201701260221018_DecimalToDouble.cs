namespace EbaySalesTracker.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DecimalToDouble : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE dbo.ListingTransactions DROP CONSTRAINT DF__ListingTr__Total__6383C8BA");
            Sql("ALTER TABLE dbo.ListingTransactions DROP CONSTRAINT DF__ListingTr__UnitP__6477ECF3");
            Sql("ALTER TABLE dbo.ListingTransactions DROP CONSTRAINT DF__ListingTr__Buyer__656C112C");
            Sql("ALTER TABLE dbo.ListingTransactions DROP CONSTRAINT DF__ListingTr__Buyer__66603565"); 


            AlterColumn("dbo.ListingTransactions", "TotalAmountPaid", c => c.Double(nullable: false));
            AlterColumn("dbo.ListingTransactions", "UnitPrice", c => c.Double(nullable: false));
            AlterColumn("dbo.ListingTransactions", "BuyerShippingCost", c => c.Double(nullable: false));
            AlterColumn("dbo.ListingTransactions", "BuyerHandlingCost", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ListingTransactions", "BuyerHandlingCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ListingTransactions", "BuyerShippingCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ListingTransactions", "UnitPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ListingTransactions", "TotalAmountPaid", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
