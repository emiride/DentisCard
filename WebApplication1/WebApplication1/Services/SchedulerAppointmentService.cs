using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class SchedulerAppointmentService : ISchedulerEventService<Appointment>
    {
        private static bool UpdateDatabase = false;
        private ApplicationDbContext db;

        public SchedulerAppointmentService()
        {
            db = new ApplicationDbContext();
        }
        public void Delete(Appointment appointment, ModelStateDictionary modelState)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public void Insert(Appointment appointment, ModelStateDictionary modelState)
        {
            if (ValidateModel(appointment,modelState))
            {
                if (!UpdateDatabase)
                {
                    var first = GetAll().OrderByDescending(e => e.Id).FirstOrDefault();
                    var id = (first != null) ? first.Id : "0";

                    appointment.Id = id + 1;
                    GetAll().ToList().Insert(0, appointment);
                }
                else
                {
                    if (string.IsNullOrEmpty(appointment.Title))
                    {
                        appointment.Title = "";
                    }
                    
                }
                
            }
        }

       

        public void Update(Appointment appointment, ModelStateDictionary modelState)
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<Appointment> GetAll()
        {
            bool isWebApiRequest = HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith("~/api");

            IQueryable<Appointment> result = null;

            //if (!isWebApiRequest)
            //{
            //    result = HttpContext.Current.Session["SchedulerAppointments"] as IQueryable<Appointment>;
            //}

            if (result == null || UpdateDatabase)
            {
                result = db.Appointments;

                if (!isWebApiRequest)
                {
                    HttpContext.Current.Session["SchedulerTasks"] = result;
                }
            }
            var list = result.ToList();
            return result;
        }

        private bool ValidateModel(Appointment appointment, ModelStateDictionary modelState)
        {
            if (appointment.Start > appointment.End)
            {
                modelState.AddModelError("errors", "End date must be greater or equal to start date");
                return false;
            }
            return true;
        }
    }
}