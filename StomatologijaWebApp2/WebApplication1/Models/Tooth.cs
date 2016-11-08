using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class Tooth
    {
        public int Id { get; set; }



        //Relations
        public MedicalHistory MedicalHistory { get; set; }
        public ICollection<MedicalRecord> MedicalRecords { get; set; }
    }
}