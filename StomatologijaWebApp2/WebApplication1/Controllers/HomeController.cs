using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebApplication1.Enumerations;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult LandingPage()
        {
            var context = new ApplicationDbContext();
            var passwordHasher = new PasswordHasher();
            var patient = new WebApplication1.Models.Patient
            {
                Id = "1",
                UserName = "asdas",
                PasswordHash = passwordHasher.HashPassword("polamarke"),
                FirstName = "Pacijent",
                LastName = "Bolesni",
                Password = "fening",
                DateOfBirth = new DateTime(1987, 4, 3),
                Address = "Brcanska 2",
                DateCreated = DateTime.Now,
                EmploymentStatus = EmploymentStatus.Employed,
                //DateModified = new DateTime(2015, 5, 19, 13, 45, 0),
                Email = "cetvrti@gmail.ocami",
                PhoneNumber = "062/064-064"
                
                // Dentist =             needs to be implemented...? 
                // EmploymentStatus =    needs to be implemented...? 
                // MedicalRecords =      needs to be implemented...? 
            };
            var dentist = new Dentist
            {
                FirstName = "Prvi",
                LastName = "Doktor",
                PasswordHash = passwordHasher.HashPassword("polamarke"),
                DateOfBirth = new DateTime(1977, 4, 3),
                EmploymentStatus = EmploymentStatus.Employed,
                PhoneNumber = "062/062-062",
                Email = "prvi@gmail.ocami",
                Address = "Kulovica 9",
                DateCreated = DateTime.Now,
                UserName = "asddas",
                Password = passwordHasher.HashPassword("polamarke"),
                Patients = new List<Patient> {patient}
            };
            context.Users.AddOrUpdate(dentist);
            //context.Users.AddOrUpdate(patient);

            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
                var x = 2;
            }

            return View();
        }
    }
}