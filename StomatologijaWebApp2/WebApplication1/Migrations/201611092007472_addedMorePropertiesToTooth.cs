namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedMorePropertiesToTooth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teeth", "ToothPosition", c => c.Int(nullable: false));
            AddColumn("dbo.Teeth", "ToothState", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Teeth", "ToothState");
            DropColumn("dbo.Teeth", "ToothPosition");
        }
    }
}
