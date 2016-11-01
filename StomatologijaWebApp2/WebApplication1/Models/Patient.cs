using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Password)] //DataType is very powerfull Data Annotation, which can affect our view if we use EF, so I will try to accomplish as much as possible with that
        public string Password { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        public EmploymentStatus? EmploymentStatus { get; set; } //Question mark means that this value is nullable

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Address { get; set; }


        //The following are the relations to other classes (One patient can have one Dentist and many MedicalRecords)

        public int? DentistId { get; set; }

        public virtual Dentist Dentist { get; set; }

        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; }

    }

}