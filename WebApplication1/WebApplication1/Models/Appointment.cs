using Kendo.Mvc.UI;
using System;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Interfaces;

namespace WebApplication1.Models
{
    public class Appointment : IModificationHistory, ISchedulerEvent
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Title can't be more than 50 characters")]
        public string Title { get; set; }
        [Required]
        [StringLength(3000, ErrorMessage = "Description can't be more than 3000 characters")]
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool IsAllDay { get; set; }
        public string Recurrence { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }
        public string StartTimezone { get; set; }

        public string EndTimezone { get; set; }


        public string ScheduleId { get; set; }
        public string PatientId { get; set; }

        //Relations
        public virtual Schedule Schedule { get; set; }
        public virtual Patient Patient { get; set; }


    }
}