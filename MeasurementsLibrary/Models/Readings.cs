//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MeasurementsLibrary.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Readings
    {
        public System.Guid Id { get; set; }
        public System.DateTime MeasurementDateTime { get; set; }
        public decimal Value { get; set; }
        public int DeviceId { get; set; }
    
        public virtual Devices Devices { get; set; }
    }
}
