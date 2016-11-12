using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModels.Dentist;

namespace WebApplication1.Controllers
{
    public class DentistController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Dentist
        public ActionResult Index(string id)
        {
            //List<Patient> patients = db.Dentists.Select(p => new Patient {Id = p.Id}).ToList();
            //var applicationUsers = db.Dentists;

            //var dentists = db.Dentists;


            var viewModel = new DentistIndexData();
            viewModel.Dentists = db.Dentists.Include(i => i.Patients);

            if (id != "")
            {
                ViewBag.DentistId = id;
                viewModel.Patients = viewModel.Dentists.Single(i => i.Id == id).Patients;
            }

            //var patients = db.Patients;
            return View(viewModel);
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
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,DateOfBirth,Address,EmploymentStatus,DateCreated,DateModified,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Place")] Dentist dentist)
        {
            if (ModelState.IsValid)
            {
                db.Dentists.Add(dentist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.Schedules, "Id", "Id", dentist.Id);
            return View(dentist);
        }

        // GET: Dentist/Edit/5
        public ActionResult Edit(string id)
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
            ViewBag.Id = new SelectList(db.Schedules, "Id", "Id", dentist.Id);
            return View(dentist);
        }

        // POST: Dentist/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,DateOfBirth,Address,EmploymentStatus,DateCreated,DateModified,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Place")] Dentist dentist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dentist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.Schedules, "Id", "Id", dentist.Id);
            return View(dentist);
        }

        // GET: Dentist/Delete/5
        public ActionResult Delete(string id)
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
