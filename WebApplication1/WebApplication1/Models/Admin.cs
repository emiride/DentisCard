using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    [Table("Admin")]
    public class Admin : ApplicationUser
    {
        //Relation
        public virtual ICollection<Note> Notes { get; set; }
        public virtual ICollection<Dentist> Dentists { get; set; }

    }
}