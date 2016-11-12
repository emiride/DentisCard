namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdminAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admin",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        comment = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
            AddColumn("dbo.Dentist", "Admin_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Dentist", "Admin_Id");
            AddForeignKey("dbo.Dentist", "Admin_Id", "dbo.Admin", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Dentist", "Admin_Id", "dbo.Admin");
            DropForeignKey("dbo.Admin", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.Dentist", new[] { "Admin_Id" });
            DropIndex("dbo.Admin", new[] { "Id" });
            DropColumn("dbo.Dentist", "Admin_Id");
            DropTable("dbo.Admin");
        }
    }
}
