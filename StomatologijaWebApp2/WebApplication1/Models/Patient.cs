using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{

    public enum EmploymentStatus
    {
        Employed = 1,
        Unemployed = 2,
        Student = 3,
        Retired = 4
    }
    public class Patient
    {
        //The following are the properties of the Patient class
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public EmploymentStatus? EmploymentStatus { get; set; } //Question mark means that this value is nullable
        public string PhoneNumber { get; set; }
        public string Address { get; set; }


        //The following are the relations to other classes (One patient can have one Dentist and many MedicalRecords)
        public virtual Dentist Dentist { get; set; }
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; }

    }

}