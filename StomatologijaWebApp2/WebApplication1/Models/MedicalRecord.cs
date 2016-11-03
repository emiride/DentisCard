using System;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Interfaces;

namespace WebApplication1.Models
{
    public class MedicalRecord : IModificationHistory
    {
        [Required]
        public  int Id { get; set; }

        [Required]
        [StringLength(3000, ErrorMessage = "Description must be less than 3000 letters")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        [DataType(DataType.Currency)]//This Data Annotation is in case we decide to use Internationalization later on (http://bitly.com/2fDF7L9)
        [Display(Name = "Bill")]
        public double Bill { get; set; }
        
        //Relations
        public int PatientId { get; set; }//For some reason, when we are modelling relation where each of the Medical records can be assigned to just one Patient, we need PatientId and Patient itself
        public Patient Patient { get; set; }

        
    }
}