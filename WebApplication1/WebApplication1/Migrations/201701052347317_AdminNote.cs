namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdminNote : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                        Title = c.String(),
                        Comment = c.String(),
                        Admin_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admin", t => t.Admin_Id)
                .Index(t => t.Admin_Id);
            
            DropColumn("dbo.Admin", "comment");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Admin", "comment", c => c.String());
            DropForeignKey("dbo.Notes", "Admin_Id", "dbo.Admin");
            DropIndex("dbo.Notes", new[] { "Admin_Id" });
            DropTable("dbo.Notes");
        }
    }
}
