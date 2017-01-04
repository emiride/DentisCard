using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class PatientProfileViewModel
    {
        public MedicalRecord MedicalRecord { get; set; }
        public Tooth Tooth { get; set; }
        public Patient Patient { get; set; }
        public MedicalHistory MedicalHistory { get; set; }
        public IEnumerable<MedicalRecord> MedicalRecords { get; set; }
        public IEnumerable<MedicalRecord> MedicalRecords1 { get; set; }
        public IEnumerable<Tooth> Teeth { get; set; }
        
    }

}