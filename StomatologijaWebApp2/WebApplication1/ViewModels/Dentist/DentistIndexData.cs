using System.Collections.Generic;
using WebApplication1.Models;

namespace WebApplication1.ViewModels.Dentist
{
    public class DentistIndexData
    {
        public IEnumerable<Models.Dentist> Dentists { get; set; }
        public IEnumerable<Patient> Patients { get; set; }
    }
}