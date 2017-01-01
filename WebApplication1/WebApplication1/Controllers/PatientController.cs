using Kendo.Mvc.Extensions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class PatientController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ApplicationUserManager ApplicationUserManager { get; set; }
        public ApplicationSignInManager ApplicationSignInManager { get; set; }

        public PatientController()
        {

        }

        public PatientController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return ApplicationUserManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                ApplicationUserManager = value;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return ApplicationSignInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                ApplicationSignInManager = value;
            }
        }


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
        public ActionResult MedicalHistory()
        {
            PatientProfileViewModel patientProfile = new PatientProfileViewModel();
            var id = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var patient = db.Patients.Find(id);

            var medicalHistory = db.MedicalHistories.Find(id);

            var medicalRecords = db.MedicalRecords.Where(m => m.MedicalHistoryId == id).ToList();         
            var teeth = db.Teeth.Where(m => m.MedicalHistoryId == id).ToList();
           
            patientProfile.Patient = patient;
            patientProfile.MedicalHistory = medicalHistory;
            patientProfile.MedicalRecords = medicalRecords;
            patientProfile.Teeth = teeth; 

            return View(patientProfile);
        }

        [Authorize(Roles = Role.Patient)]
        public ActionResult MyTeeth()
        {
            var currentPatientId = User.Identity.GetUserId();
            //PatientProfileViewModel patientProfile = new PatientProfileViewModel();
            Patient patientProfile = new Patient();

            Patient patient = db.Patients.Find(currentPatientId);
            var medicalHistory = db.MedicalHistories.Find(currentPatientId);
            var medicalRecords = db.MedicalRecords.Where(m => m.MedicalHistoryId == currentPatientId).ToList();
            var teeth = db.Teeth.Where(m => m.MedicalHistoryId == currentPatientId).ToList();
           

            patientProfile.MedicalHistory = medicalHistory;
            patientProfile.MedicalHistory.MedicalRecords = medicalRecords;
            patientProfile.MedicalHistory.Teeth = teeth;

            //patientProfile.Patient = patient;
            //patientProfile.MedicalHistory = medicalHistory;
            //patientProfile.MedicalRecords = medicalRecords;
            //patientProfile.Teeth = teeth;
            return View(patientProfile);

        }


        //Edit method for Patient editig his/her profile
        [Authorize(Roles = Role.User+","+Role.Patient)]
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
        [Authorize(Roles = Role.User + "," + Role.Patient)]
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

        [Authorize(Roles = Role.User)]
        public ActionResult ChooseDentist(string id)
        {
            var CurrentUserId = User.Identity.GetUserId();
            var CurrentPatient = new Patient();
            var patients = db.Patients;
            foreach(var p in patients)
            {
                if (p.Id == CurrentUserId)
                {
                    CurrentPatient = p;
                }
            }
            UserManager.RemoveFromRole(CurrentUserId, Role.User);
            db.SaveChanges();
            if (ModelState.IsValid)
            {
                Dentist ChoosenDentist = db.Dentists.Find(id);
                var DentistId = ChoosenDentist.Id;
                CurrentPatient.DentistId = DentistId;
                UserManager.AddToRole(CurrentUserId, Role.Patient);              
                db.SaveChanges();
                var user = UserManager.FindById(User.Identity.GetUserId());
                SignInManager.SignIn(user, false, false);
                return RedirectToAction("Index", "Patient");
            }

            return View();
        }


        [Authorize(Roles = Role.User)]
        public ActionResult PickDentist(string sortOrder, string searchString)
        {
            List<Dentist> dentistsList = new List<Dentist>();
            var dentists = db.Dentists;
            foreach (var i in dentists)
            {
                dentistsList.Add(i);
            }
            //Sorting
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.LNameSortParm = string.IsNullOrEmpty(sortOrder) ? "LName_desc" : "LName";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.BDateSortParm = sortOrder == "BDate" ? "Bdate_desc" : "BDate";

            var patientOrder = from s in dentistsList
                               select s;
            if (!string.IsNullOrEmpty(searchString))
            {
                patientOrder = patientOrder.Where(s => s.FirstName.ToLower().Contains(searchString.ToLower()) || s.LastName.ToLower().Contains(searchString.ToLower()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    patientOrder = patientOrder.OrderByDescending(s => s.FirstName);
                    break;
                case "LName":
                    patientOrder = patientOrder.OrderBy(s => s.LastName);
                    break;
                case "LName_desc":
                    patientOrder = patientOrder.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    patientOrder = patientOrder.OrderBy(s => s.DateCreated);
                    break;
                case "date_desc":
                    patientOrder = patientOrder.OrderByDescending(s => s.DateCreated);
                    break;
                case "BDate":
                    patientOrder = patientOrder.OrderBy(s => s.DateOfBirth);
                    break;
                case "Bdate_desc":
                    patientOrder = patientOrder.OrderByDescending(s => s.DateOfBirth);
                    break;
                default:
                    patientOrder = patientOrder.OrderBy(s => s.FirstName);
                    break;
            }

            return View(patientOrder);
        }

        public ActionResult SeeDentistProfile(string id)
        {
            Dentist ChoosenDentist = db.Dentists.Find(id);
            return View(ChoosenDentist);
        }
    }
}