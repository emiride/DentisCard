using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Enumerations;

namespace WebApplication1.Controllers
{
    [Authorize (Roles = Role.Admin)]
    public class AdminController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin
        public ActionResult Index()
        {
            /*
            var viewModel = new DentistIndexData();
            viewModel.Dentists = db.Dentists.Include(i => i.Patients);

            if (id != "")
            {
                ViewBag.DentistId = id;
                /*viewModel.Patients = viewModel.Dentists.Single(i => i.Id == id).Patients;
            }

            //var patients = db.Patients;
            return View(viewModel);
            */
            var currentAdminId = User.Identity.GetUserId();
            var admins = db.Admins;
            var admin = new Admin();
            foreach (var d in admins)
            {
                if (d.Id == currentAdminId)
                {
                    admin = d;
                }
            }
            return View(admin);
        }

        // GET: Admin/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }
        
        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,DateOfBirth,Address,EmploymentStatus,DateCreated,DateModified,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,comment")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,DateOfBirth,Address,EmploymentStatus,DateCreated,DateModified,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,comment")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admin);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Admin admin = db.Admins.Find(id);
            db.Admins.Remove(admin);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize(Roles = Role.Admin)]
        public ActionResult GetDentist(string sortOrder, string searchString)
        {
            List<Dentist> dentistList = new List<Dentist>();

            var dentists = db.Dentists;
            foreach (var dentist in dentists)
            {
                dentistList.Add(dentist);
            }

            //Sorting
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.LNameSortParm = String.IsNullOrEmpty(sortOrder) ? "LName_desc" : "LName";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.BDateSortParm = sortOrder == "BDate" ? "Bdate_desc" : "BDate";

            var patientOrder = from s in dentistList
                               select s;
            if (!String.IsNullOrEmpty(searchString))
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

        [Authorize(Roles = Role.Admin)]
        public ActionResult GetPatients(string sortOrder, string searchString)
        {
            List<Patient> patientList = new List<Patient>();

            var patients = db.Patients;
            foreach (var patient in patients)
            {
                    patientList.Add(patient);
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

        public ActionResult Updates()
        {
            return View();
        }


        [HttpPost, ActionName("Updates")]
        [ValidateAntiForgeryToken]
        public ActionResult Updates(Note model)
        {

            var adminId = User.Identity.GetUserId();

            var note = new Note
            {
                DateCreated = DateTime.Now,
                Title = model.Title, 
                Comment = model.Comment,
                Id = Guid.NewGuid().ToString()
                
            };
            List<Note> j = new List<Note>();
            j.Add(note);

            if (ModelState.IsValid)
            {
                var admin = db.Admins.Find(adminId);
                admin.Notes = j;   
                db.SaveChanges();
                return RedirectToAction("Updates");

            }
            return View();
        }

        public ActionResult DeleteUpdate(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note= db.Notes.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUpdate2(string id)
        {
            Note note = db.Notes.Find(id);

            db.Notes.Remove(note);

            db.SaveChanges();

            return RedirectToAction("ListAllUpdates");
        }

        [Authorize]
        public ActionResult EditUpdates()
        {
            //all comments
            List<Note> noteList = new List<Note>();
            var notes = db.Notes.OrderByDescending(p => p.DateCreated);
            foreach (var k in notes)
            {
                noteList.Add(k);
            }

            return View(noteList);

        }

    }
}
