using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using WebApplication1.ViewModels.Dentist;
using System.Data;

namespace WebApplication1.Controllers
{
    public class PatientController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Patient
        public ActionResult Index()
        {
            var currentPatientId = User.Identity.GetUserId();
            var patients = db.Patients;
            var patient = new Patient();
            foreach (var p in patients)
            {
                if (p.Id == currentPatientId)
                {
                    patient = p;
                }
            }
            return View(patient);
        }

        public ActionResult FindDoctor()
        {
            return View();
        }

        //Allowed just for Patient user
        [Authorize(Roles = Role.Patient)]
        public ActionResult ArrangeAppointment()
        {
            return View();
        }

        //Allowed just for Patient user
        [Authorize(Roles = Role.Patient)]
        public ActionResult MyDentist()
        {
            return View();
        }

        [Authorize(Roles = Role.Patient)]
        public ActionResult MedicalHistory(string sortOrder, string searchString)
        {
            List<MedicalHistory> historyList = new List<MedicalHistory>();

            var medicalHistory = db.MedicalHistories;
            foreach (var note in medicalHistory)
            {
                historyList.Add(note);
            }

            /* TODO sorting
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.LNameSortParm = String.IsNullOrEmpty(sortOrder) ? "LName_desc" : "LName";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.BDateSortParm = sortOrder == "BDate" ? "Bdate_desc" : "BDate";
            */
            var patientOrder = from s in historyList
                               select s;
            /*
             * TODO print medical history nicely
            */

            return View();
        }

        [Authorize(Roles = Role.Patient)]
        public ActionResult Edit()
        {
            var currentPatientId = User.Identity.GetUserId();
            var patients = db.Patients;
            var CurrentPatient = new Patient();
            foreach (var d in patients)
            {
                if (d.Id == currentPatientId)
                {
                    CurrentPatient = d;
                }
            }
            return View(CurrentPatient);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,DateOfBirth,Address,EmploymentStatus,DateCreated,DateModified,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Place")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.Schedules, "Id", "Id", patient.Id);
            return View(patient);
        }

    }
}