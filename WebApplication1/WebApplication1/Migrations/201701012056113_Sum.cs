namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Patient", "SumBills", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Patient", "SumBills");
        }
    }
}
