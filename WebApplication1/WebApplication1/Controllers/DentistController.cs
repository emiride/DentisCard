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
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Enumerations;
using WebApplication1.Models;
using WebApplication1.ViewModels;
using WebApplication1.ViewModels.Dentist;


namespace WebApplication1.Controllers
{
    [Authorize(Roles = Role.Dentist)]
    public class DentistController : BaseController
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
            DentistIndexViewModel dentistIndex = new DentistIndexViewModel();

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
            var patients = db.Patients.ToList().Where(m => m.DentistId == currentDentistId && m.IsApproved == true).ToList();

            dentistIndex.Dentist = dentist;
            dentistIndex.Patients = patients;
            return View(dentistIndex);
        }

        // GET: Patients
        
        public ActionResult MyPatients(string sortOrder, string searchString)
        {
            var dentistId = User.Identity.GetUserId();
            var patientList = db.Patients.ToList().Where(m => m.DentistId == dentistId && m.IsApproved == true).ToList();


            //Sorting
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.LNameSortParm = string.IsNullOrEmpty(sortOrder) ? "LName_desc" : "LName";
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
            var appointments = db.Appointments;
            return View(appointments);
        }

        public ActionResult MyAppointments(string sortOrder, string searchString)
        {
            var dentistId = User.Identity.GetUserId();
            var pavm = new List<PatientAppointmentViewModel>();
            var query = (from appointment in db.Appointments
                join patient in db.Patients on appointment.PatientId equals patient.Id
                        where patient.DentistId == dentistId where appointment.IsAccepted == false
                        orderby appointment.Start descending 
                select new {appointment.Id, appointment.Title, appointment.Start, appointment.End, appointment.Description, patient.FirstName, patient.LastName, patient.Email}).ToList();
            foreach (var item in query)
            {
                pavm.Add(new PatientAppointmentViewModel
                {
                    Id = item.Id,
                    Description = item.Description,
                    End = item.End,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Start = item.Start,
                    Title = item.Title,
                    Email = item.Email

                });
            }
            //Sorting
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.LNameSortParm = string.IsNullOrEmpty(sortOrder) ? "LName_desc" : "LName";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.BDateSortParm = sortOrder == "BDate" ? "Bdate_desc" : "BDate";

            var appointmentOrder = from s in pavm
                               select s;
            if (!string.IsNullOrEmpty(searchString))
            {
                appointmentOrder = appointmentOrder.Where(s => s.FirstName.ToLower().Contains(searchString.ToLower()) || s.LastName.ToLower().Contains(searchString.ToLower()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    appointmentOrder = appointmentOrder.OrderByDescending(s => s.FirstName);
                    break;
                case "LName":
                    appointmentOrder = appointmentOrder.OrderBy(s => s.LastName);
                    break;
                case "LName_desc":
                    appointmentOrder = appointmentOrder.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    appointmentOrder = appointmentOrder.OrderBy(s => s.Start);
                    break;
                case "date_desc":
                    appointmentOrder = appointmentOrder.OrderByDescending(s => s.Start);
                    break;
                case "BDate":
                    appointmentOrder = appointmentOrder.OrderBy(s => s.End);
                    break;
                case "Bdate_desc":
                    appointmentOrder = appointmentOrder.OrderByDescending(s => s.End);
                    break;
                default:
                    appointmentOrder = appointmentOrder.OrderBy(s => s.Start);
                    break;
            }

            return View(pavm);
        }

        public ActionResult AcceptAppointment(string id)
        {
            var appointment = db.Appointments.Find(id);
            appointment.IsAccepted = true;
            db.SaveChanges();
            var task = SendEmail("");
            
            SendEmail("bla");
            return RedirectToAction("MyAppointments");
        }
        public ActionResult RejectAppointment(string id)
        {

            return View();
        }
        public ActionResult Notes()
        {
            return View();
        }

        public ActionResult Draft()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendEmail(string email)
        {
            //var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
            var message = new MailMessage();
            message.To.Add(new MailAddress("emir.hodzich@gmail.com"));
            message.From = new MailAddress("emir.hodzic@outlook.com");
            message.Subject = "Proba";
            message.Body = "Proba jos jednom";
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "emir.hodzic@outlook.com",  
                    Password = "Vrl0T@j@nP@ssw0rd"  
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp-mail.outlook.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(message);
                
            }
            return View();
        }
        public ActionResult PatientRead([DataSourceRequest] DataSourceRequest request)
        {
            var dentistId = User.Identity.GetUserId();

            var patients = db.Patients;
            List<Patient> patientList = patients.Where(patient => patient.DentistId == dentistId).ToList();

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
                patient.IsApproved = false;

                //creating default values for teeth
                var positions = Enum.GetValues(typeof(ToothPosition)).Cast<ToothPosition>().ToList();
                var teeth = new List<Tooth>();

                for (int i = 0; i < positions.Count; i++)
                {
                    teeth.Add(new Tooth
                    {
                        ToothPosition = positions[i],
                        ToothState = ToothState.H,
                    });
                }
                patient.MedicalHistory = new MedicalHistory
                {
                    Teeth = teeth,
                    //PatientId = patient.Id,
                    
                };
                db.Patients.Add(patient);
                db.SaveChanges();
                UserManager.AddToRole(patient.Id, Role.Patient);
                db.SaveChanges();

                return RedirectToAction("MyPatients");
            }

            return View(patient);
        }

        // GET: Dentist/Edit/5
        [Authorize(Roles = Role.Dentist)]
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
        [Authorize(Roles = Role.Dentist)]
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
        public ActionResult Delete(string id)
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

        //[Authorize(Roles = Role.Dentist)]
        //public ActionResult PatientProfile()
        //{
        //    return View();
        //}


        [Authorize(Roles = Role.Dentist)]
        [HttpPost, ActionName("PatientProfile")]
        [ValidateAntiForgeryToken]
        public ActionResult InsertMedicalRecord2(string id, PatientProfileViewModel model)
        {
            var p = db.Patients.Find(id);
            var PozicijaZuba = model.Tooth.ToothPosition;
            var StanjeZuba = model.Tooth.ToothState;


            var medicalRecord02 = new MedicalRecord
            {
                DateCreated = DateTime.Now,
                Description = model.MedicalRecord.Description,
                Bill = model.MedicalRecord.Bill,
                ToothPosition = PozicijaZuba,
                ToothState = StanjeZuba

            };


            var history = db.MedicalHistories.Find(id);
            var zubi = history.Teeth;

            foreach (var i in zubi)
            {
                if (i.ToothPosition == PozicijaZuba)
                {
                    i.ToothState = StanjeZuba;

                }
            }

            if (ModelState.IsValid)
            {
                var MedicalRecords = new List<MedicalRecord>() { medicalRecord02 };
                p.MedicalHistory.MedicalRecords = MedicalRecords;
                db.SaveChanges();
                return RedirectToAction("PatientProfile");
            }

            return View();
        }


        [Authorize(Roles = Role.Dentist)]
        public ActionResult MedicalRecord()
        {
            return View();
        }


        [Authorize(Roles = Role.Dentist)]
        [HttpPost, ActionName("MedicalRecord")]
        [ValidateAntiForgeryToken]
        public ActionResult InsertMedicalRecord(string id, PatientProfileViewModel model)
        {
           
            var p = db.Patients.Find(id);
            var PozicijaZuba = model.Tooth.ToothPosition;
            var StanjeZuba = model.Tooth.ToothState;
           
          
            var medicalRecord02 = new MedicalRecord
            {
                DateCreated = DateTime.Now,
                Description = model.MedicalRecord.Description,
                Bill = model.MedicalRecord.Bill,
                ToothPosition = PozicijaZuba,
                ToothState= StanjeZuba

            };

          
            var history = db.MedicalHistories.Find(id);
            var zubi = history.Teeth;

            foreach (var i in zubi)
            {
                if (i.ToothPosition==PozicijaZuba)
                {
                    i.ToothState= StanjeZuba;
                    
                }
            }

            if (ModelState.IsValid) { 
            var MedicalRecords = new List<MedicalRecord>() { medicalRecord02 };
            p.MedicalHistory.MedicalRecords =MedicalRecords;      
            db.SaveChanges();
            return RedirectToAction("MyPatients");
            }
            return View();
        }

        [Authorize(Roles = Role.Dentist)]
        public ActionResult ListAllMedicalRecords(string id, PatientProfileViewModel model)
        {
            PatientProfileViewModel patientProfile = new PatientProfileViewModel();
            //ovdje redom pravim kverije na bazu i ona mi vraca podatke, te podatke spremim i na kraju proslijedim u view
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var patient = db.Patients.Find(id);
            
            var medicalHistory = db.MedicalHistories.Find(id);

            var AllmedicalRecords = db.MedicalRecords.Where(m => m.MedicalHistoryId == id).ToList();
            var medicalRecords = AllmedicalRecords.OrderByDescending(p => p.DateCreated);
            var teeth = db.Teeth.Where(m => m.MedicalHistoryId == id).ToList();

            double bills = 0.0;
            foreach(var item in medicalRecords)
            {
                if(item.Bill > 1.0)
                {
                    bills = bills + item.Bill;
                }
            }

            //sve njih, strpamo u ovaj viewmodel koji smo prethodno napravili
            patientProfile.Patient = patient;
            patientProfile.MedicalHistory = medicalHistory;
            patientProfile.MedicalRecords = medicalRecords;
            patientProfile.Teeth = teeth;
            patientProfile.Patient.SumBills = bills;
            return View(patientProfile);
        }

        [Authorize(Roles = Role.Dentist)]
        public ActionResult Requests(string sortOrder, string searchString)
        {
            var dentistId = User.Identity.GetUserId();

            var patientRequestList = db.Patients.ToList().Where(m => m.DentistId == dentistId && m.IsApproved==false).ToList();
           
   

            //Sorting
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.LNameSortParm = string.IsNullOrEmpty(sortOrder) ? "LName_desc" : "LName";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.BDateSortParm = sortOrder == "BDate" ? "Bdate_desc" : "BDate";

            var patientOrder = from s in patientRequestList
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


        [Authorize (Roles = Role.Dentist)]
        public ActionResult PatientProfile(string id){

            PatientProfileViewModel patientProfile = new PatientProfileViewModel();
            //ovdje redom pravim kverije na bazu i ona mi vraca podatke, te podatke spremim i na kraju proslijedim u view
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var patient = db.Patients.Find(id);

            var medicalHistory = db.MedicalHistories.Find(id);

            var medicalRecords = db.MedicalRecords.Where(m => m.MedicalHistoryId == id).ToList();

            var teeth = db.Teeth.Where(m => m.MedicalHistoryId == id).ToList();

            var lastTwoRecords = medicalRecords.OrderByDescending(p => p.DateCreated).Take(2);
            var lastRecord = medicalRecords.OrderByDescending(p => p.DateCreated).Take(1);


            //sve njih, strpamo u ovaj viewmodel koji smo prethodno napravili
            patientProfile.Patient = patient;
            patientProfile.MedicalHistory = medicalHistory;
            patientProfile.MedicalRecords = lastTwoRecords;
            patientProfile.MedicalRecords1 = lastRecord;
            patientProfile.Teeth = teeth;
            



            //MedicalHistory medicalHistory = db.MedicalHistories.Find(id);
            //if (medicalHistory == null)
            //{
            //    return HttpNotFound();
            //}

            //MedicalRecord medicalRecord = db.MedicalRecords.Find(id);
            //if (medicalRecord == null)
            //{
            //    return HttpNotFound();
            //}

            //Tooth Teeth = db.Teeth.Find(id);

            /*TODO*/

            /*PatientProfile patientProfile = new PatientProfile { Patient = patient, MedicalHistory = medicalHistory };

            return View(patientProfile);*/

            //i na kraju posaljemo taj viewmodel u view    
            return View(patientProfile);
        }


        [Authorize(Roles = Role.Dentist)]       
        public ActionResult ToothMedicalRecord(string id, ToothPosition position)
        {
            PatientProfileViewModel patientProfile = new PatientProfileViewModel();
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToothPosition newPosition = position;
            var patient = db.Patients.Find(id);

            var medicalHistory = db.MedicalHistories.Find(id);

            var medicalRecords = db.MedicalRecords.Where(m => m.MedicalHistoryId == id).ToList();
            var ToothMedicalRecord = medicalRecords.Where(m => m.ToothPosition == newPosition);
            double bills = 0.0;
            foreach (var item in ToothMedicalRecord)
            {
                if (item.Bill > 1.0)
                {
                    bills = bills + item.Bill;
                }
            }


            patientProfile.Patient = patient;
            patientProfile.MedicalHistory = medicalHistory;
            patientProfile.MedicalRecords = ToothMedicalRecord;
            patientProfile.Patient.SumBills = bills;

            return View(patientProfile);          
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
        [Authorize(Roles = Role.Dentist)]
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

        public ActionResult ApprovePatient(string id)
        {
            var currentUserId = id;
            var currentPatient = db.Patients.Find(id);

            currentPatient.IsApproved = true;
            db.SaveChanges();
            if (ModelState.IsValid)
            {
                if (currentPatient.IsApproved == true)
                {
                    if (currentPatient.IsApproved == true)
                    {
                        UserManager.RemoveFromRole(currentUserId, Role.User);
                        db.SaveChanges();
                    }
                    UserManager.AddToRole(currentUserId, Role.Patient);
                    db.SaveChanges();
                    // var user = UserManager.FindById(User.Identity.GetUserId());
                    //SignInManager.SignIn(user, false, false);
                    // return RedirectToAction("Index", "Patient");
                    return RedirectToAction("MyPatients");
                }            
                
            }
            return View();
        }
    }
}
