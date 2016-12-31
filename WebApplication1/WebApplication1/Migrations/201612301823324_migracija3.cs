namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migracija3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MedicalRecords", "MDTeeth_Id", "dbo.MRTeeth");
            DropIndex("dbo.MedicalRecords", new[] { "MDTeeth_Id" });
            DropColumn("dbo.MedicalRecords", "MDTeeth_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MedicalRecords", "MDTeeth_Id", c => c.Int());
            CreateIndex("dbo.MedicalRecords", "MDTeeth_Id");
            AddForeignKey("dbo.MedicalRecords", "MDTeeth_Id", "dbo.MRTeeth", "Id");
        }
    }
}
