namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migracija2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.MedicalRecords", name: "Teeth_Id", newName: "Tooth_Id");
            RenameIndex(table: "dbo.MedicalRecords", name: "IX_Teeth_Id", newName: "IX_Tooth_Id");
            CreateTable(
                "dbo.MRTeeth",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ToothPosition = c.Int(nullable: false),
                        ToothState = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.MedicalRecords", "MDTeeth_Id", c => c.Int());
            AddColumn("dbo.MedicalRecords", "MRTeeth_Id", c => c.Int());
            CreateIndex("dbo.MedicalRecords", "MDTeeth_Id");
            CreateIndex("dbo.MedicalRecords", "MRTeeth_Id");
            AddForeignKey("dbo.MedicalRecords", "MDTeeth_Id", "dbo.MRTeeth", "Id");
            AddForeignKey("dbo.MedicalRecords", "MRTeeth_Id", "dbo.MRTeeth", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MedicalRecords", "MRTeeth_Id", "dbo.MRTeeth");
            DropForeignKey("dbo.MedicalRecords", "MDTeeth_Id", "dbo.MRTeeth");
            DropIndex("dbo.MedicalRecords", new[] { "MRTeeth_Id" });
            DropIndex("dbo.MedicalRecords", new[] { "MDTeeth_Id" });
            DropColumn("dbo.MedicalRecords", "MRTeeth_Id");
            DropColumn("dbo.MedicalRecords", "MDTeeth_Id");
            DropTable("dbo.MRTeeth");
            RenameIndex(table: "dbo.MedicalRecords", name: "IX_Tooth_Id", newName: "IX_Teeth_Id");
            RenameColumn(table: "dbo.MedicalRecords", name: "Tooth_Id", newName: "Teeth_Id");
        }
    }
}
