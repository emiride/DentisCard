using System;

namespace WebApplication1.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<WebApplication1.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebApplication1.Models.ApplicationDbContext context)
        {

            //Seeding Dentist Class
            context.Dentists.AddOrUpdate( 
                d => d.Id,
                new Models.Dentist {
                    Id = 1,
                    FirstName = "Prvi",
                    LastName = "Doktor",
                    Password = "polamarke",
                    DateOfBirth = new DateTime(1977, 4, 3),
                   // EmploymentStatus = new    seeding enum ...?
                    PhoneNumber = "062/062-062",
                    Email = "prvi@gmail.ocami",
                    Address = "Kulovica 9",
                    DateCreated = DateTime.Now,
                    //DateModified = new DateTime(2015, 5, 16, 13, 45, 0), This can be done later because I changed DateModified to be nullable, so it doesn't have to be populated now
                    //Patients          needs to be implemented...? 
                },

                new Models.Dentist { 
                    Id = 2,
                    FirstName = "Druga",
                    LastName = "Doktorka",
                    Password = "dvijemarke",
                    DateOfBirth = new DateTime(1973, 2, 3),
                    //EmploymentStatus = (1),     seeding enum ...?
                    PhoneNumber = "062/061-061",
                    Email = "drugi@gmail.ocami",
                    Address = "Ramovica 9",
                    DateCreated = DateTime.Now,
                    //DateModified = new DateTime(2015, 5, 17, 13, 45, 0),
                    //Patients         needs to be implemented...? 
                },

                new Models.Dentist {
                    Id = 3,
                    FirstName = "Treci",
                    LastName = "Doka",
                    Password = "trimarke",
                    DateOfBirth = new DateTime(1975, 8, 3),
                    //EmploymentStatus = (1),     seeding enum ...?
                    PhoneNumber = "062/063-063", 
                    Email = "treci@gmail.ocami",
                    Address = "Muvedina 8",
                    DateCreated = DateTime.Now 
                    //DateModified = new DateTime(2015, 5, 17, 12, 45, 0),
                    //Patients          needs to be implemented...? 
                }
            );
            
            //Seeding Patient Class
            context.Patients.AddOrUpdate( 
               k => k.Id,
               new Models.Patient
               {
                   Id = 1,
                   FirstName = "Pacijent",
                   LastName = "Bolesni",
                   Password = "fening",
                   DateOfBirth = new DateTime(1987, 4, 3),
                   Address = "Brcanska 2", 
                   DateCreated = DateTime.Now,
                   //DateModified = new DateTime(2015, 5, 19, 13, 45, 0),
                   Email = "cetvrti@gmail.ocami",
                   PhoneNumber = "062/064-064",
                   DentistId = 2,
                   // Dentist =             needs to be implemented...? 
                   // EmploymentStatus =    needs to be implemented...? 
                   // MedicalRecords =      needs to be implemented...? 
               },

               new Models.Patient
               {
                   Id = 2,
                   FirstName = "Drugi",
                   LastName = "Bolesnik",
                   Password = "dolar",
                   DateOfBirth = new DateTime(1997, 4, 3),
                   Address = "Malta 2",
                   DateCreated = DateTime.Now,
                   //DateModified = new DateTime(2015, 5, 19, 13, 45, 0),
                   Email = "cetvrti@gmail.ocami",
                   PhoneNumber = "062/065-065",
                   DentistId = 1,
                   // Dentist =             needs to be implemented...? 
                   // EmploymentStatus =    needs to be implemented...? 
                   // MedicalRecords =      needs to be implemented...? 
               },

               new Models.Patient
               {
                   Id = 3,
                   FirstName = "Pacijentac",
                   LastName = "Najbolesniji",
                   Password = "feningas",
                   DateOfBirth = new DateTime(1983, 4, 3),
                   Address = "Logavina 2",
                   DateCreated = DateTime.Now,
                   //DateModified = new DateTime(2015, 5, 12, 13, 45, 0),
                   Email = "peti@gmail.ocami",
                   PhoneNumber = "062/066-066",
                   DentistId = 2,
                   
                   // Dentist =             needs to be implemented...? 
                   // EmploymentStatus =    needs to be implemented...? 
                   // MedicalRecords =      needs to be implemented...? 
               }
            );

            //Seeding MedicalRecords Class
            context.MedicalRecords.AddOrUpdate( 
               x => x.Id,
               new Models.MedicalRecord
               {
                   Id = 1,
                   Description = "ovo je neki deskripšn",
                   DateCreated = DateTime.Now,
                   //DateModified = new DateTime(2015, 5, 16, 13, 45, 0),
                   Bill =  144.5,
                   PatientId = 3 
                   // EmploymentStatus =        needs to be implemented...? 
                   // MedicalRecords =          needs to be implemented...? 
                },
               
               new Models.MedicalRecord
               {
                   Id = 2,
                   Description = "ovo je neki drugi deskripšn",
                   DateCreated = DateTime.Now,
                   //DateModified = new DateTime(2015, 5, 16, 2, 45, 0),
                   Bill = 34.2, 
                   PatientId = 2
                   // Dentist =                 needs to be implemented...? 
                   // EmploymentStatus =        needs to be implemented...? 
                   // MedicalRecords =          needs to be implemented...? 
                },

               new Models.MedicalRecord
               {
                   Id = 3,
                   Description = "ovo je neki zadnji treci deskripšn",
                   DateCreated = DateTime.Now,
                   //DateModified = new DateTime(2015, 5, 17, 19, 45, 0),
                   Bill = 554.5,
                   PatientId = 2
                   // Dentist =                 needs to be implemented...? 
                   // EmploymentStatus =        needs to be implemented...? 
                   // MedicalRecords =          needs to be implemented...? 
               }
            );


        }
    }
}
