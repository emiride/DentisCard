using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class Dentist
    {
        //The following are properties of the Dentist class
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public EmploymentStatus? EmploymentStatus { get; set; } //This value is nullable
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime DateCreated { get; set; }

        //The following are the relations to other classes (One Dentist can have many Patients and we are still not dealing with schedules)
        public virtual ICollection<Patient> Patients { get; set; }
    }

}


