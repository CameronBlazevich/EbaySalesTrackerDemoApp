namespace EbaySalesTracker.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixSubscriptionPlanTableNamesArentDateTimes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SubscriptionPlans", "ModifiedBy", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SubscriptionPlans", "ModifiedBy", c => c.DateTime(nullable: false));
        }
    }
}
