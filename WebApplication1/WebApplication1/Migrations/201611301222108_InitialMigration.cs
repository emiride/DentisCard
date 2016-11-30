namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        DateOfBirth = c.DateTime(),
                        Address = c.String(),
                        EmploymentStatus = c.Int(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false, maxLength: 3000),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        IsAllDay = c.Boolean(nullable: false),
                        Recurrence = c.String(),
                        RecurrenceRule = c.String(),
                        RecurrenceException = c.String(),
                        StartTimezone = c.String(),
                        EndTimezone = c.String(),
                        ScheduleId = c.String(maxLength: 128),
                        PatientId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Patient", t => t.PatientId)
                .ForeignKey("dbo.Schedules", t => t.ScheduleId)
                .Index(t => t.ScheduleId)
                .Index(t => t.PatientId);
            
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DentistId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dentist", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.MedicalHistories",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Note = c.String(),
                        PatientId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Patient", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.MedicalRecords",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Description = c.String(nullable: false, maxLength: 3000),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(),
                        Bill = c.Double(nullable: false),
                        MedicalHistory_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MedicalHistories", t => t.MedicalHistory_Id)
                .Index(t => t.MedicalHistory_Id);
            
            CreateTable(
                "dbo.Teeth",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ToothPosition = c.Int(nullable: false),
                        ToothState = c.Int(nullable: false),
                        MedicalHistory_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MedicalHistories", t => t.MedicalHistory_Id)
                .Index(t => t.MedicalHistory_Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.ToothMedicalRecords",
                c => new
                    {
                        Tooth_Id = c.Int(nullable: false),
                        MedicalRecord_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Tooth_Id, t.MedicalRecord_Id })
                .ForeignKey("dbo.Teeth", t => t.Tooth_Id, cascadeDelete: true)
                .ForeignKey("dbo.MedicalRecords", t => t.MedicalRecord_Id, cascadeDelete: true)
                .Index(t => t.Tooth_Id)
                .Index(t => t.MedicalRecord_Id);
            
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
            
            CreateTable(
                "dbo.Dentist",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Admin_Id = c.String(maxLength: 128),
                        Place = c.String(),
                        PatientId = c.String(),
                        ScheduleId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Admin", t => t.Admin_Id)
                .Index(t => t.Id)
                .Index(t => t.Admin_Id);
            
            CreateTable(
                "dbo.Patient",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DentistId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Dentist", t => t.DentistId)
                .Index(t => t.Id)
                .Index(t => t.DentistId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Patient", "DentistId", "dbo.Dentist");
            DropForeignKey("dbo.Patient", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Dentist", "Admin_Id", "dbo.Admin");
            DropForeignKey("dbo.Dentist", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Admin", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.MedicalHistories", "Id", "dbo.Patient");
            DropForeignKey("dbo.ToothMedicalRecords", "MedicalRecord_Id", "dbo.MedicalRecords");
            DropForeignKey("dbo.ToothMedicalRecords", "Tooth_Id", "dbo.Teeth");
            DropForeignKey("dbo.Teeth", "MedicalHistory_Id", "dbo.MedicalHistories");
            DropForeignKey("dbo.MedicalRecords", "MedicalHistory_Id", "dbo.MedicalHistories");
            DropForeignKey("dbo.Schedules", "Id", "dbo.Dentist");
            DropForeignKey("dbo.Appointments", "ScheduleId", "dbo.Schedules");
            DropForeignKey("dbo.Appointments", "PatientId", "dbo.Patient");
            DropIndex("dbo.Patient", new[] { "DentistId" });
            DropIndex("dbo.Patient", new[] { "Id" });
            DropIndex("dbo.Dentist", new[] { "Admin_Id" });
            DropIndex("dbo.Dentist", new[] { "Id" });
            DropIndex("dbo.Admin", new[] { "Id" });
            DropIndex("dbo.ToothMedicalRecords", new[] { "MedicalRecord_Id" });
            DropIndex("dbo.ToothMedicalRecords", new[] { "Tooth_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Teeth", new[] { "MedicalHistory_Id" });
            DropIndex("dbo.MedicalRecords", new[] { "MedicalHistory_Id" });
            DropIndex("dbo.MedicalHistories", new[] { "Id" });
            DropIndex("dbo.Schedules", new[] { "Id" });
            DropIndex("dbo.Appointments", new[] { "PatientId" });
            DropIndex("dbo.Appointments", new[] { "ScheduleId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropTable("dbo.Patient");
            DropTable("dbo.Dentist");
            DropTable("dbo.Admin");
            DropTable("dbo.ToothMedicalRecords");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Teeth");
            DropTable("dbo.MedicalRecords");
            DropTable("dbo.MedicalHistories");
            DropTable("dbo.Schedules");
            DropTable("dbo.Appointments");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
        }
    }
}
