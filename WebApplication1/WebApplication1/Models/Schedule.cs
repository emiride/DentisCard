﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Schedule
    {
        public Schedule()
        {
            Id = Guid.NewGuid().ToString();
        }
        [ForeignKey("Dentist")]
        public string Id { get; set; }

        
        public string DentistId { get; set; }
        //Relations
        public virtual Dentist Dentist { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}