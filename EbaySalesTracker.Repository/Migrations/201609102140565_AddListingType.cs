namespace EbaySalesTracker.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddListingType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Listings", "Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Listings", "Type");
        }
    }
}
