namespace EbaySalesTracker.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStripesColumnsToUserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "StripeCustomerId", c => c.String(maxLength: 500));
            AddColumn("dbo.AspNetUsers", "StripeActiveUntil", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "CreditCardExpires", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "CreditCardExpires");
            DropColumn("dbo.AspNetUsers", "StripeActiveUntil");
            DropColumn("dbo.AspNetUsers", "StripeCustomerId");
        }
    }
}
