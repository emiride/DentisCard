using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class MedicalHistory
    {
        
        [ForeignKey("Patient")]
        public string Id { get; set; }
        public string Note { get; set; }

        public string PatientId { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual ICollection<Tooth> Teeth { get; set; }
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; }
    }
}