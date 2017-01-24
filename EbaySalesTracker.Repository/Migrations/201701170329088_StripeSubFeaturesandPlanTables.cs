namespace EbaySalesTracker.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StripeSubFeaturesandPlanTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubscriptionFeatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlanId = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 100),
                        DisplayOrder = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SubscriptionPlans", t => t.PlanId, cascadeDelete: true)
                .Index(t => t.PlanId);
            
            CreateTable(
                "dbo.SubscriptionPlans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExternalId = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false, maxLength: 500),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubscriptionFeatures", "PlanId", "dbo.SubscriptionPlans");
            DropIndex("dbo.SubscriptionFeatures", new[] { "PlanId" });
            DropTable("dbo.SubscriptionPlans");
            DropTable("dbo.SubscriptionFeatures");
        }
    }
}
