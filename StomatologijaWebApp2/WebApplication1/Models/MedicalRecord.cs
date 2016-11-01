using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class MedicalRecord
    {
        [Required]
        public  int Id { get; set; }

        public string Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public double Bill { get; set; }//dont know what annotation could add, maybe none

        //gledao sam ovo kako si ti radio ovo u EF al spominju se nakvi kljucevi pa nisam htio nista dodavati da ne zabrljam sta
    }
}