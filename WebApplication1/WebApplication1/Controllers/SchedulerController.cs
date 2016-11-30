using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Web.Mvc;
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

        protected override void Dispose(bool disposing)
        {
            appointmentService.Dispose();
            
            base.Dispose(disposing);
        }
    }
}