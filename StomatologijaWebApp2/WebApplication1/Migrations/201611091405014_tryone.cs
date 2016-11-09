namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tryone : DbMigration
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
                        Password = c.String(nullable: false),
                        DateOfBirth = c.DateTime(nullable: false),
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
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false, maxLength: 3000),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(),
                        Patient_Id = c.String(maxLength: 128),
                        Schedule_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Patient", t => t.Patient_Id)
                .ForeignKey("dbo.Schedules", t => t.Schedule_Id)
                .Index(t => t.Patient_Id)
                .Index(t => t.Schedule_Id);
            
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Patient", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.MedicalRecords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
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
                        MedicalRecord_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tooth_Id, t.MedicalRecord_Id })
                .ForeignKey("dbo.Teeth", t => t.Tooth_Id, cascadeDelete: true)
                .ForeignKey("dbo.MedicalRecords", t => t.MedicalRecord_Id, cascadeDelete: true)
                .Index(t => t.Tooth_Id)
                .Index(t => t.MedicalRecord_Id);
            
            CreateTable(
                "dbo.Dentist",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Place = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Patient",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Dentist_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.Dentist", t => t.Dentist_Id)
                .Index(t => t.Id)
                .Index(t => t.Dentist_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Patient", "Dentist_Id", "dbo.Dentist");
            DropForeignKey("dbo.Patient", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Dentist", "Id", "dbo.AspNetUsers");
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
            DropForeignKey("dbo.Appointments", "Schedule_Id", "dbo.Schedules");
            DropForeignKey("dbo.Appointments", "Patient_Id", "dbo.Patient");
            DropIndex("dbo.Patient", new[] { "Dentist_Id" });
            DropIndex("dbo.Patient", new[] { "Id" });
            DropIndex("dbo.Dentist", new[] { "Id" });
            DropIndex("dbo.ToothMedicalRecords", new[] { "MedicalRecord_Id" });
            DropIndex("dbo.ToothMedicalRecords", new[] { "Tooth_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Teeth", new[] { "MedicalHistory_Id" });
            DropIndex("dbo.MedicalRecords", new[] { "MedicalHistory_Id" });
            DropIndex("dbo.MedicalHistories", new[] { "Id" });
            DropIndex("dbo.Schedules", new[] { "Id" });
            DropIndex("dbo.Appointments", new[] { "Schedule_Id" });
            DropIndex("dbo.Appointments", new[] { "Patient_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropTable("dbo.Patient");
            DropTable("dbo.Dentist");
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
