namespace EbaySalesTracker.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class listingProfitField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Listings", "Profit", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Listings", "Profit");
        }
    }
}
