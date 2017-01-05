using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.ViewModels.Dentist
{
    public class DentistIndexViewModel
    {
        public Models.Dentist Dentist { get; set; }
        public List<Patient> Patients { get; set; }
    }
}