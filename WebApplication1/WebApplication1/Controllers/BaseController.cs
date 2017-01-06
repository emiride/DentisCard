using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class BaseController : Controller
    {
        public List<Appointment> Appointment;
        public List<Patient> Patients { get; set; }
        public BaseController()
        {
            var db = new ApplicationDbContext();
            var currentDentistId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            Appointment = db.Appointments.Where(a => a.IsAccepted == false && a.ScheduleId == currentDentistId).ToList();
            ViewBag.appointmentRequests = Appointment;

            Patients = db.Patients.Where(p => p.IsApproved == false && p.DentistId == currentDentistId).ToList();
            ViewBag.patientRequests = Patients;

        }
        

    }
}