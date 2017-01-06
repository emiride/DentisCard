using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Interfaces;
using WebApplication1.Enumerations;

namespace WebApplication1.Models
{
    public class Note
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        public string Title { get; set; }
        public string Id { get; set; }
        public string Comment { get; set; }

    }
}