using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{

    [Table("Dentist")]
    public class Dentist : ApplicationUser
    {


        //Relations
        public virtual ICollection<Patient> Patients { get; set; }
        public virtual Schedule Schedule { get; set; }
    }

}


