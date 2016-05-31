namespace EbaySalesTracker.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TokenExpDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "TokenExpDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "TokenExpDate");
        }
    }
}
