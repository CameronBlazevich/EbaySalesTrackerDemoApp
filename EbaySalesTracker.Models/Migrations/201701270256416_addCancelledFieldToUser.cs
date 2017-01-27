namespace EbaySalesTracker.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCancelledFieldToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Cancelled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Cancelled");
        }
    }
}
