namespace EbaySalesTracker.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createWebHookLogsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WebhookLogs",
                c => new
                    {
                        EventId = c.String(nullable: false, maxLength: 128),
                        StripeUserId = c.String(),
                        UserId = c.String(),
                        CreatedDate = c.DateTime(),
                        EventType = c.String(),
                    })
                .PrimaryKey(t => t.EventId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WebhookLogs");
        }
    }
}
