using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class DentistsListOfPatient
    {
        public IEnumerable<Models.Dentist> Dentists { get; set; }
        public IEnumerable<Patient> Patients { get; set; }
    }

}