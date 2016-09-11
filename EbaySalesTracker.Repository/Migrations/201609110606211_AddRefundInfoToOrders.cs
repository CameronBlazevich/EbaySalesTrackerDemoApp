namespace EbaySalesTracker.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRefundInfoToOrders : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "RefundAmount", c => c.Double());
            AddColumn("dbo.Orders", "RefundTime", c => c.DateTime());
            AddColumn("dbo.Orders", "RefundStatus", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "RefundStatus");
            DropColumn("dbo.Orders", "RefundTime");
            DropColumn("dbo.Orders", "RefundAmount");
        }
    }
}
