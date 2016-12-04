using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModels;
using WebApplication1.ViewModels.Dentist;


namespace WebApplication1.Controllers
{
    [Authorize(Roles = Role.Dentist)]
    public class DentistController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ApplicationUserManager ApplicationUserManager { get; set; }
        public ApplicationSignInManager ApplicationSignInManager { get; set; }

        public DentistController()
        {
            
        }

        public DentistController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        public ActionResult Index()
        {
            var currentDentistId = User.Identity.GetUserId();
            var dentists = db.Dentists;
            var dentist = new Dentist();
            foreach(var d in dentists)
            {
                if(d.Id == currentDentistId)
                {
                    dentist = d;
                }
            }
            return View(dentist);
        }

        // GET: Patients
        
        public ActionResult MyPatients(string sortOrder, string searchString)
        {
            var dentistId = User.Identity.GetUserId();
            List<Patient> patientList = new List<Patient>();

            var patients = db.Patients;
            foreach (var patient in patients)
            {
                if (patient.DentistId == dentistId)
                {
                    patientList.Add(patient);
                }
            }

            //Sorting
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.LNameSortParm = String.IsNullOrEmpty(sortOrder) ? "LName_desc" : "LName";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.BDateSortParm = sortOrder == "BDate" ? "Bdate_desc" : "BDate";

            var patientOrder = from s in patientList
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

        public ActionResult MySchedule()
        {
            Schedule schedule = new Schedule();
            schedule.Appointments = new List<Appointment>
            {
                new Appointment() {
                Title = "Fast and furious 6",
                Start = new DateTime(2013,6,13,17,00,00),
                End= new DateTime(2013,6,13,18,30,00)
            },
            new Appointment() {
                Title= "The Internship",
                Start= new DateTime(2013,6,13,14,00,00),
                End= new DateTime(2013,6,13,15,30,00)
            },
            new Appointment() {
                Title = "The Perks of Being a Wallflower",
                Start =  new DateTime(2013,6,13,16,00,00),
                End =  new DateTime(2013,6,13,17,30,00)
            }
            };
            return View(schedule.Appointments);
        }

        public ActionResult Notes()
        {
            return View();
        }

        public ActionResult Draft()
        {
            return View();
        }

        public ActionResult PatientRead([DataSourceRequest] DataSourceRequest request)
        {
            var dentistId = User.Identity.GetUserId();
            List<Patient> patientList = new List<Patient>();

            var patients = db.Patients;
            foreach (var patient in patients)
            {
                if (patient.DentistId == dentistId)
                {
                    patientList.Add(patient);
                }
            }

            DataSourceResult result = patientList.ToDataSourceResult(request, p => new Patient
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Address = p.Address,
                Email = p.Email,
            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: Dentist/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // GET: Dentist/Create
        public ActionResult Create()
        {
            //ViewBag.Id = new SelectList(db.Schedules, "Id", "Id");
            return View();
        }

        // POST: Dentist/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName,LastName,DateOfBirth,Address,EmploymentStatus,DateCreated,DateModified,Email,PhoneNumber")] Patient patient)
        {
            var passwordHasher = new PasswordHasher();
            var dentistId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                patient.Id = Guid.NewGuid().ToString();
                patient.PasswordHash = passwordHasher.HashPassword("P@ssw0rd");
                patient.UserName = patient.Email;
                patient.DentistId = dentistId;
                db.Patients.Add(patient);
                db.SaveChanges();
                UserManager.AddToRole(patient.Id, Role.Patient);
                db.SaveChanges();

                return RedirectToAction("MyPatients");
            }

            //ViewBag.Id = new SelectList(db.Schedules, "Id", "Id", dentist.Id);
            return View(patient);
        }

        // GET: Dentist/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.Schedules, "Id", "Id", patient.Id);
            return View(patient);
        }

        // POST: Dentist/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPatients(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var PatientToUpdate = db.Patients.Find(id);
            if (TryUpdateModel(PatientToUpdate, "",
               new string[] { "FirstName", "LastName", "DateOfBirth", "Address", "EmploymentStatus", "Email", "PhoneNumber", "UserName"}))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("MyPatients");
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(PatientToUpdate);
        }

        // POST: Dentist/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Patient patient = db.Patients.Find(id);
            //var medicalHistory = db.MedicalHistories.Where(pid => pid.PatientId == id).ToList();

            db.MedicalRecords.RemoveRange(patient.MedicalHistory.MedicalRecords);
            db.MedicalHistories.Remove(patient.MedicalHistory);
            db.Patients.Remove(patient);
            
            db.SaveChanges();
            return RedirectToAction("MyPatients");
        }

        [Authorize(Roles = Role.Dentist)]
        public ActionResult PatientProfile()
        {
            return View();
        }

        
        [Authorize (Roles = Role.Dentist)]
        public ActionResult PatientProfile(string id){
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }

            MedicalHistory medicalHistory = db.MedicalHistories.Find(id);
            if (medicalHistory == null)
            {
                return HttpNotFound();
            }

            MedicalRecord medicalRecord = db.MedicalRecords.Find(id);
            if (medicalRecord == null)
            {
                return HttpNotFound();
            }

            Tooth Teeth = db.Teeth.Find(id);
            
                /*TODO*/

                /*PatientProfile patientProfile = new PatientProfile { Patient = patient, MedicalHistory = medicalHistory };

                return View(patientProfile);*/
                return View();
        }


        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(DentistRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Dentist
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.FirstName,
                    Email = model.Email,
                    DateCreated = DateTime.Now
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, Role.Dentist);
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Dentist");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }




       

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //za prikazivanje podataka nakon klika na "edit profile" za dentist
        [Authorize (Roles = Role.Dentist)]
        public ActionResult EditMyProfile()
        {
            var currentDentistId = User.Identity.GetUserId();
            var dentists = db.Dentists;
            var CurrentDentist = new Dentist();
            foreach (var d in dentists)
            {
                if (d.Id == currentDentistId)
                {
                    CurrentDentist = d;
                }
            }
            return View(CurrentDentist);
        }
        
        //asinhrona metoda za spremanje podataka u bazu nakon editovanja
        [HttpPost, ActionName("EditMyProfile")]
        [ValidateAntiForgeryToken]
        public ActionResult EditMyProfile(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dentistToUpdate = db.Dentists.Find(id);
            if (TryUpdateModel(dentistToUpdate, "",
               new string[] {  "FirstName", "LastName", "DateOfBirth", "Address", "EmploymentStatus", "Email", "PhoneNumber", "UserName", "UserPhoto" }))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(dentistToUpdate);
        }

    }
}
