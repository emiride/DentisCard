namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mog : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MedicalRecords", "MRTeeth_Id", "dbo.MRTeeth");
            DropIndex("dbo.MedicalRecords", new[] { "MRTeeth_Id" });
            AddColumn("dbo.MedicalRecords", "ToothPosition", c => c.Int(nullable: false));
            AddColumn("dbo.MedicalRecords", "ToothState", c => c.Int(nullable: false));
            DropColumn("dbo.MedicalRecords", "MRTeeth_Id");
            DropTable("dbo.MRTeeth");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.MRTeeth",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ToothPosition = c.Int(nullable: false),
                        ToothState = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.MedicalRecords", "MRTeeth_Id", c => c.Int());
            DropColumn("dbo.MedicalRecords", "ToothState");
            DropColumn("dbo.MedicalRecords", "ToothPosition");
            CreateIndex("dbo.MedicalRecords", "MRTeeth_Id");
            AddForeignKey("dbo.MedicalRecords", "MRTeeth_Id", "dbo.MRTeeth", "Id");
        }
    }
}
