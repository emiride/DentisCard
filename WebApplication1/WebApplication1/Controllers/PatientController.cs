using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PatientController : Controller
    {
        // GET: Patient
        public ActionResult Index()
        {
            return View();
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
    }
}