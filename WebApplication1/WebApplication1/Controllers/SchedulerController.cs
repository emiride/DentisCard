using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class SchedulerController : Controller
    {
        private readonly SchedulerAppointmentService appointmentService;

        public SchedulerController()
        {
            appointmentService = new SchedulerAppointmentService();
        }

        // GET: Scheduler
        public ActionResult Index()
        {
            return View();
        }
        public virtual JsonResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = new List<Appointment>
            {
                new Appointment()
                    {
                        Start = new DateTime(2016, 11, 28, 8, 0, 0),
                        End = new DateTime(2016, 11, 28, 9, 0, 0),
                        Title = "Task 1"
                    }
            };



            return Json(appointmentService.GetAll().ToList().ToDataSourceResult(request), JsonRequestBehavior.AllowGet); 
        }

        protected override void Dispose(bool disposing)
        {
            appointmentService.Dispose();
            
            base.Dispose(disposing);
        }
    }
}