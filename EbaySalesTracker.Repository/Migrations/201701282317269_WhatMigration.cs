namespace EbaySalesTracker.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WhatMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ListingTransactions", "OrderId", c => c.String(maxLength: 128));
            CreateIndex("dbo.ListingTransactions", "OrderId");
            AddForeignKey("dbo.ListingTransactions", "OrderId", "dbo.Orders", "OrderId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ListingTransactions", "OrderId", "dbo.Orders");
            DropIndex("dbo.ListingTransactions", new[] { "OrderId" });
            AlterColumn("dbo.ListingTransactions", "OrderId", c => c.String());
        }
    }
}
