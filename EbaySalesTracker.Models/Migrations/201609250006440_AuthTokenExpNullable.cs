namespace EbaySalesTracker.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuthTokenExpNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "TokenExpDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "TokenExpDate", c => c.DateTime(nullable: false));
        }
    }
}
