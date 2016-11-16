using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class PatientService : ApiController
    {
        private readonly ApplicationDbContext context = new ApplicationDbContext();

        public PatientService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<PatientDTO> GetAllPatients()
        {
            var patients = from p in context.Patients
                select new PatientDTO
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    BirthDate = p.DateOfBirth,
                    DateCreated = p.DateCreated
                };
            return patients.ToList();
        }

        public IEnumerable<PatientDTO> GetAllMyPatients()
        {
            var dentistId = User.Identity.GetUserId();
            var patients = from p in context.Patients
                           where p.DentistId == dentistId
                           select new PatientDTO
                           {
                               Id = p.Id,
                               FirstName = p.FirstName,
                               LastName = p.LastName,
                               BirthDate = p.DateOfBirth,
                               DateCreated = p.DateCreated

                           };

            return patients.ToList();
        }

        [ResponseType(typeof(Patient))]
        public IHttpActionResult GetPatient(string id)
        {
            Patient patient = context.Patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }

            return Ok(patient);
        }

        public IHttpActionResult Create(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            patient.DentistId = User.Identity.GetUserId();
            context.Patients.Add(patient);

            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PatientExists(patient.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = patient.Id }, patient);
        }
        [ResponseType(typeof(void))]
        public IHttpActionResult Update(string id, Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != patient.Id)
            {
                return BadRequest();
            }

            context.Entry(patient).State = EntityState.Modified;

            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
        [ResponseType(typeof(Patient))]
        public IHttpActionResult Destroy(string id)
        {
            Patient patient = context.Patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }

            context.Patients.Remove(patient);
            context.SaveChanges();

            return Ok(patient);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
        private bool PatientExists(string id)
        {
            return context.Patients.Count(e => e.Id == id) > 0;
        }
    }
}
