using System.Collections.Generic;
using WebApplication1.Enumerations;

namespace WebApplication1.Models
{
    public class Tooth
    {
        public int Id { get; set; }

        public ToothPosition ToothPosition { get; set; }

        public ToothState ToothState { get; set; }

        //Relations
        public MedicalHistory MedicalHistory { get; set; }
        public ICollection<MedicalRecord> MedicalRecords { get; set; }
    }
}