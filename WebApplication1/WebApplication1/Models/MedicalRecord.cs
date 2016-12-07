using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Interfaces;

namespace WebApplication1.Models
{
    public class MedicalRecord : IModificationHistory
    {
        public MedicalRecord()
        {
            Id = Guid.NewGuid().ToString();
        }
        [Required]
        public string Id { get; set; }

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

        public string MedicalHistoryId { get; set; }
        
        //Relations
        public MedicalHistory MedicalHistory { get; set; }
        public ICollection<Tooth> Teeth { get; set; }
        
    }
}