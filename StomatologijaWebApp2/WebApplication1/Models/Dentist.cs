using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApplication1.Interfaces;

namespace WebApplication1.Models
{
    public class Dentist : IModificationHistory
    {
        //The following are properties of the Dentist class
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Password)] //DataType is very powerfull Data Annotation, which can affect our view if we use EF, so I will try to accomplish as much as possible with that
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }

        public EmploymentStatus? EmploymentStatus { get; set; } //This value is nullable

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        //The following are the relations to other classes (One Dentist can have many Patients and we are still not dealing with schedules)
        public virtual ICollection<Patient> Patients { get; set; }
    }

}


