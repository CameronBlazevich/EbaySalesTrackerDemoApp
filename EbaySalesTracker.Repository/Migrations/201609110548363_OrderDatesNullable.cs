namespace EbaySalesTracker.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderDatesNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "PaidTime", c => c.DateTime());
            AlterColumn("dbo.Orders", "ShippedTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "ShippedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Orders", "PaidTime", c => c.DateTime(nullable: false));
        }
    }
}
