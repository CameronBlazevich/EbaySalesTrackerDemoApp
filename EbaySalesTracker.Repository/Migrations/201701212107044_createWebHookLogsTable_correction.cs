namespace EbaySalesTracker.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createWebHookLogsTable_correction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WebhookLogs", "LoggedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WebhookLogs", "LoggedDate");
        }
    }
}
