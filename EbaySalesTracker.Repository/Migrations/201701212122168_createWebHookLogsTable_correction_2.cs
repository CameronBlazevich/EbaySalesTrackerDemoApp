namespace EbaySalesTracker.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createWebHookLogsTable_correction_2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.WebhookLogs", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WebhookLogs", "UserId", c => c.String());
        }
    }
}
