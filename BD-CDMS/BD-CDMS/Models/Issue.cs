//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BD_CDMS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Issue
    {
        public int Id { get; set; }
        public int IdCar { get; set; }
        public Nullable<int> IdServiceman { get; set; }
        public string Description { get; set; }
    
        public virtual Car Car { get; set; }
        public virtual Serviceman Serviceman { get; set; }
    }
}
