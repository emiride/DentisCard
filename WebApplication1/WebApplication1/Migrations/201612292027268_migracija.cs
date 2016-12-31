namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migracija : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ToothMedicalRecords", "Tooth_Id", "dbo.Teeth");
            DropForeignKey("dbo.ToothMedicalRecords", "MedicalRecord_Id", "dbo.MedicalRecords");
            DropIndex("dbo.ToothMedicalRecords", new[] { "Tooth_Id" });
            DropIndex("dbo.ToothMedicalRecords", new[] { "MedicalRecord_Id" });
            AddColumn("dbo.MedicalRecords", "Teeth_Id", c => c.Int());
            CreateIndex("dbo.MedicalRecords", "Teeth_Id");
            AddForeignKey("dbo.MedicalRecords", "Teeth_Id", "dbo.Teeth", "Id");
            DropTable("dbo.ToothMedicalRecords");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ToothMedicalRecords",
                c => new
                    {
                        Tooth_Id = c.Int(nullable: false),
                        MedicalRecord_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Tooth_Id, t.MedicalRecord_Id });
            
            DropForeignKey("dbo.MedicalRecords", "Teeth_Id", "dbo.Teeth");
            DropIndex("dbo.MedicalRecords", new[] { "Teeth_Id" });
            DropColumn("dbo.MedicalRecords", "Teeth_Id");
            CreateIndex("dbo.ToothMedicalRecords", "MedicalRecord_Id");
            CreateIndex("dbo.ToothMedicalRecords", "Tooth_Id");
            AddForeignKey("dbo.ToothMedicalRecords", "MedicalRecord_Id", "dbo.MedicalRecords", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ToothMedicalRecords", "Tooth_Id", "dbo.Teeth", "Id", cascadeDelete: true);
        }
    }
}
