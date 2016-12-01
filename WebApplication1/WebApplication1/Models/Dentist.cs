using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{

    [Table("Dentist")]
    public class Dentist : ApplicationUser
    {

        public string Place { get; set; }

        public string PatientId { get; set; }
        public string ScheduleId { get; set; }

        //Relations
        public virtual ICollection<Patient> Patients { get; set; }
        public virtual Schedule Schedule { get; set; }
    }

}


