using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public void Dispose()
        {
            db.Dispose();
        }

        public void Insert(Appointment appointment, ModelStateDictionary modelState)
        {
            if (!ValidateModel(appointment, modelState)) return;

            var scheduleId = HttpContext.Current.User.Identity.GetUserId();
            appointment.Id = Guid.NewGuid().ToString();
            appointment.ScheduleId = scheduleId;

            db.Appointments.Add(appointment);
            db.SaveChanges();
        }


        public void Update(Appointment appointment, ModelStateDictionary modelState)
        {
            if (!ValidateModel(appointment, modelState)) return;

            var target = db.Appointments.FirstOrDefault(a => a.Id == appointment.Id);
            
            if (target != null)
            {
                target.Title = appointment.Title;
                target.Start = appointment.Start;
                target.End = appointment.End;
                target.StartTimezone = appointment.StartTimezone;
                target.EndTimezone = appointment.EndTimezone;
                target.Description = appointment.Description;
                target.IsAllDay = appointment.IsAllDay;
                target.RecurrenceRule = appointment.RecurrenceRule;
                target.RecurrenceException = appointment.RecurrenceException;
                target.Recurrence = appointment.Recurrence;
                target.PatientId = appointment.PatientId;
            }
            db.Appointments.Attach(target);
            db.Entry(target).State = EntityState.Modified;
            db.SaveChanges();

        }

        internal void RequestAppointment(Appointment appointment, ModelStateDictionary modelState)
        {
            //TODO
        }

        public void Delete(Appointment appointment, ModelStateDictionary modelState)
        {
            if (!ValidateModel(appointment, modelState)) return;

            var target = db.Appointments.FirstOrDefault(a => a.Id == appointment.Id);

            db.Appointments.Remove(target);
            db.SaveChanges();
        }

        /*This function has no use, but to implement the stupid interface given by Kendo. 
         The interface asks to implement a method that returns IQueryable, even though
         in demos on their website they are using a method that return IList*/
        public virtual IQueryable<Appointment> GetAll()
        {
            
                var result = db.Appointments.Select(a => new Appointment
                {
                    Id = a.Id,
                    Title = a.Title,
                    Start = a.Start,
                    End = a.End,
                    StartTimezone = a.StartTimezone,
                    EndTimezone = a.EndTimezone,
                    Description = a.Description,
                    IsAllDay = a.IsAllDay,
                    RecurrenceRule = a.RecurrenceRule,
                    RecurrenceException = a.RecurrenceException,
                    Recurrence = a.Recurrence,
                    ScheduleId = a.ScheduleId,
                    PatientId = a.PatientId,
                    Patient = a.Patient,
                    DateCreated = a.DateCreated,
                    Schedule = a.Schedule,
                    DateModified = a.DateModified
                });
            return result;
        }
        
        public List<Appointment> GetList()
        {
            List<Appointment> result = new List<Appointment>();

            if (HttpContext.Current.User.IsInRole(Role.Dentist))
            {
                var scheduleId = HttpContext.Current.User.Identity.GetUserId();
                result = db.Appointments.ToList().Select(a => new Appointment
                {
                    Id = a.Id,
                    Title = a.Title,
                    Start = a.Start,
                    End = a.End,
                    StartTimezone = a.StartTimezone,
                    EndTimezone = a.EndTimezone,
                    Description = a.Description,
                    IsAllDay = a.IsAllDay,
                    RecurrenceRule = a.RecurrenceRule,
                    RecurrenceException = a.RecurrenceException,
                    Recurrence = a.Recurrence,
                    ScheduleId = a.ScheduleId,
                    PatientId = a.PatientId

                }).Where(a => a.ScheduleId == scheduleId).ToList();
            }
            /*Don't try to understand this -_- :P */
            else if (HttpContext.Current.User.IsInRole(Role.Patient))
            {
                var patientId = HttpContext.Current.User.Identity.GetUserId();
                var patient = db.Patients.FirstOrDefault(p => p.Id == patientId);
                var scheduleId = "";
                if (patient != null)
                {
                    scheduleId = patient.DentistId;
                }

                result = db.Appointments.ToList().Select(a => new Appointment
                {
                    Id = a.Id,
                    Title = a.Title,
                    Start = a.Start,
                    End = a.End,
                    StartTimezone = a.StartTimezone,
                    EndTimezone = a.EndTimezone,
                    Description = a.Description,
                    IsAllDay = a.IsAllDay,
                    RecurrenceRule = a.RecurrenceRule,
                    RecurrenceException = a.RecurrenceException,
                    Recurrence = a.Recurrence,
                    ScheduleId = a.ScheduleId,
                    PatientId = a.PatientId

                }).Where(a => a.ScheduleId == scheduleId).ToList();
            }

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