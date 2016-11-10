using System.Collections.Generic;
using WebApplication1.Enumerations;

namespace WebApplication1.Models
{
    public class Tooth
    {
        public int Id { get; set; }

        public ToothPosition ToothPosition { get; set; }

        public ToothState ToothState { get; set; }
        //Something has to be added to make enum work beacuse it is Description (there are other ways to to this like using toString
        //but as i could see this way is the best one. It is a bit late so i did not fully understand so i ll leave it for next time
        //But i have to check something. For more details http://stackoverflow.com/questions/388483/how-do-you-create-a-dropdownlist-from-an-enum-in-asp-net-mvc
        //and also there are many on stackoverflow, if somebody knows how to do this i will accept help :P, or it is just copy-paste :D

        //Relations
        public MedicalHistory MedicalHistory { get; set; }
        public ICollection<MedicalRecord> MedicalRecords { get; set; }
    }
}