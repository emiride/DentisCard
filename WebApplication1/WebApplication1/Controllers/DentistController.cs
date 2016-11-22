using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModels.Dentist;
using System.Linq;

namespace WebApplication1.Controllers
{
    [Authorize]
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
            return View();
        }

        // GET: Dentist         Ti si ovaj dio koda izbrisao pa da ja sad ne trazim gdje si ga stavio i kako ja sam ga vratio 
        //dok ne proradi pa ti onda mjenjaj poslije.
        [Authorize (Roles = Role.Dentist)]
        public ActionResult MyPatients(string sortOrder, string searchString)
        {
            var dentistID = User.Identity.GetUserId();
            List<Patient> patientList = new List<Patient>();

            var patients = db.Patients;
            foreach (var patient in patients)
            {
                if (patient.DentistId == dentistID)
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
            if (!String.IsNullOrEmpty(searchString))
            {
                patientOrder = patientOrder.Where(s => s.FirstName.ToLower().Contains(searchString.ToLower()) || s.LastName.Contains(searchString.ToLower()));
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


        [Authorize(Roles = Role.Dentist)]
        public ActionResult MySchedule()
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
            Dentist dentist = db.Dentists.Find(id);
            if (dentist == null)
            {
                return HttpNotFound();
            }
            return View(dentist);
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

                return RedirectToAction("Index");
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

        // GET: Dentist/Delete/5
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
            Dentist dentist = db.Dentists.Find(id);
            db.Dentists.Remove(dentist);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Dentist/Register
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
    }
}
