using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class Dentist
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public EmploymentStatus? EmploymentStatus { get; set; } //This value is nullable
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime DateCreated { get; set; }
    }
}


