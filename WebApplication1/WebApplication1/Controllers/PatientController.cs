using Kendo.Mvc.Extensions;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class PatientController : BaseController
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
            var CurrentPatientId = User.Identity.GetUserId();
            var CurrentPatient = new Patient();
            var patients = db.Patients;
            foreach (var p in patients)
            {
                if (p.Id == CurrentPatientId)
                {
                    CurrentPatient = p;
                }
            }


            var den = CurrentPatient.DentistId;
            var PatientDentist = new Dentist();
            var dentists = db.Dentists;
            foreach(var d in dentists)
            {
                if (d.Id == den)
                {
                    PatientDentist = d;
                }
            }
            return View(PatientDentist);
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
        public ActionResult MyTeeth()
        {
            var currentPatientId = User.Identity.GetUserId();
            PatientProfileViewModel patientProfile = new PatientProfileViewModel();

            var patient = db.Patients.Find(currentPatientId);
            var medicalHistory = db.MedicalHistories.Find(currentPatientId);
            var medicalRecords = db.MedicalRecords.Where(m => m.MedicalHistoryId == currentPatientId).ToList();
            var teeth = db.Teeth.Where(m => m.MedicalHistoryId == currentPatientId).ToList();

            patientProfile.Patient = patient;
            patientProfile.MedicalHistory = medicalHistory;
            patientProfile.MedicalRecords = medicalRecords;
            patientProfile.Teeth = teeth;
            return View(patientProfile);

        }
       

        //Edit method for Patient editig his/her profile
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



        //Edit for patient asinhrona metoda
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var PatientToUpdate = db.Patients.Find(id);
            if (TryUpdateModel(PatientToUpdate, "",
               new string[] { "FirstName", "LastName", "DateOfBirth", "Address", "EmploymentStatus", "Email", "PhoneNumber", "UserName", "UserPhoto" }))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (DataException /* dex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(PatientToUpdate);
        }

        
    }
}