using System;

namespace WebApplication1.ViewModels.Dentist
{
    public class PatientAppointmentViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Description { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


    }
}