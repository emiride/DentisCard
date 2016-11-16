using System;

namespace WebApplication1.Models
{
    public class PatientDTO

    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime DateCreated { get; set; }


    }
}