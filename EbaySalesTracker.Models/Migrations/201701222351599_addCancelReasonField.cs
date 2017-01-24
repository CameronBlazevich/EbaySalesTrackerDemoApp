namespace EbaySalesTracker.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCancelReasonField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "CancelReason", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "CancelReason");
        }
    }
}
