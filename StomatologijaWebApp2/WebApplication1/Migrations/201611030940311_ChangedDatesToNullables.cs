namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedDatesToNullables : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Dentists", "DateModified", c => c.DateTime());
            AlterColumn("dbo.Patients", "DateModified", c => c.DateTime());
            AlterColumn("dbo.MedicalRecords", "DateModified", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MedicalRecords", "DateModified", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Patients", "DateModified", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Dentists", "DateModified", c => c.DateTime(nullable: false));
        }
    }
}
