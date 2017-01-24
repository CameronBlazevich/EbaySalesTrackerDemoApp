namespace EbaySalesTracker.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeIdForWebhooks : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.WebhookLogs");
            AddColumn("dbo.WebhookLogs", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.WebhookLogs", "EventId", c => c.String());
            AddPrimaryKey("dbo.WebhookLogs", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.WebhookLogs");
            AlterColumn("dbo.WebhookLogs", "EventId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.WebhookLogs", "Id");
            AddPrimaryKey("dbo.WebhookLogs", "EventId");
        }
    }
}
