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
        Healthy = 1,
        [Description("Caries and one surface filling ")]
        Caries1 = 2,
        [Description("Two surface fillings")]
        Caries2 = 3,
        [Description("Three and more fillings")]
        Caries3 = 4,
        [Description("Curing")] //I left this one as Descriptios because we may add something 
        curing = 5,
        [Description("One canal curing")]
        CanalCuring1 = 6,
        [Description("Two canals curings")]
        CanalCuring2 = 7,
        [Description("Three canals curings (possible extraction)")]
        CanalCuring3 = 8,
        [Description("No tooth")]
        NoTooth = 9,
        //ne sjecam se sta je deseti trebao biti




    }
}