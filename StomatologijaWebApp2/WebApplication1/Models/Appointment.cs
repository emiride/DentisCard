using System;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Interfaces;

namespace WebApplication1.Models
{
    public class Appointment : IModificationHistory
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Title can't be more than 50 characters")]
        public string Title { get; set; }
        [Required]
        [StringLength(3000, ErrorMessage = "Description can't be more than 3000 characters")]
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        //Relations
        public virtual Schedule Schedule { get; set; }
        public virtual Patient Patient { get; set; }
    }
}