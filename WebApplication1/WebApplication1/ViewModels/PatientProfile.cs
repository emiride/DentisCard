using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class PatientProfile
    {
        public Patient Patient { get; set; }
        public MedicalHistory MedicalHistory { get; set; }
        public IEnumerable<MedicalRecord> MedicalRecords { get; set; }
        public IEnumerable<Tooth> Teeth { get; set; }
    }

}