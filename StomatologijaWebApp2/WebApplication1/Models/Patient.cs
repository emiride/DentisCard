using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{

    [Table("Patient")]
    public class Patient : ApplicationUser
    {
        
        //Relations
        public virtual Dentist Dentist { get; set; }
        public string DentistId { get; set; }

        public virtual MedicalHistory MedicalHistory { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
    }

}