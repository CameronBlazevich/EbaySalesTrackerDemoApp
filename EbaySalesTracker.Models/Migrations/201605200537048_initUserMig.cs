namespace EbaySalesTracker.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initUserMig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "SessionId", c => c.String());
            AddColumn("dbo.AspNetUsers", "UserToken", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "UserToken");
            DropColumn("dbo.AspNetUsers", "SessionId");
        }
    }
}
