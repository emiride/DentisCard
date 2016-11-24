namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class smallTinyChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MedicalHistories", "PatientId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MedicalHistories", "PatientId");
        }
    }
}
