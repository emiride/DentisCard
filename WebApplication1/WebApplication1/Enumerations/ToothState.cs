using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebApplication1.Enumerations
{
    //TODO Jusuf
    //10 states must be implemented
    public enum ToothState
    {
    //TODO Jusuf
    
    //In order to make fully readable we cannot use just like before these enums beacuse state of tooth is small 'description' with few words
    //so we can use toDescription method and first part goes like this below   
        H = 1,
        [Description("Caries and one surface filling ")]
        C1 = 2,
        [Description("Two surface fillings")]
        C2 = 3,
        [Description("Three and more fillings")]
        C3 = 4,
        [Description("Curing")] //I left this one as Descriptios because we may add something 
        Cu = 5,
        [Description("One canal curing")]
        CC1 = 6,
        [Description("Two canals curings")]
        CC2 = 7,
        [Description("Three canals curings (possible extraction)")]
        CC3 = 8,
        [Description("No tooth")]
        No = 9,
        //ne sjecam se sta je deseti trebao biti




    }
}