using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var result = db.Appointments.ToList().Select(a => new Appointment
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
                
            }).ToList();

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