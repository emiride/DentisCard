using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
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
            
            return Json(appointmentService.GetList().ToDataSourceResult(request), JsonRequestBehavior.AllowGet); 
        }

        public virtual JsonResult Create([DataSourceRequest] DataSourceRequest request, Appointment appointment)
        {
            appointmentService.Insert(appointment, ModelState);

            return Json(new[] {appointment}.ToDataSourceResult(request, ModelState));
        }

        public virtual JsonResult Update([DataSourceRequest] DataSourceRequest request, Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                appointmentService.Update(appointment, ModelState);
            }

            return Json(new[] { appointment }.ToDataSourceResult(request, ModelState));
        }

        public virtual JsonResult Destroy([DataSourceRequest] DataSourceRequest request, Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                appointmentService.Delete(appointment, ModelState);
            }

            return Json(new[] { appointment }.ToDataSourceResult(request, ModelState));
        }

        public virtual JsonResult RequestAppointment([DataSourceRequest] DataSourceRequest request, Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                appointmentService.RequestAppointment(appointment, ModelState);
            }

            return Json(new[] { appointment }.ToDataSourceResult(request, ModelState));
        }

        protected override void Dispose(bool disposing)
        {
            appointmentService.Dispose();
            
            base.Dispose(disposing);
        }
    }
}