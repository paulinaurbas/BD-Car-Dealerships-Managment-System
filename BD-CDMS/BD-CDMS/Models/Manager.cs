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
    
    public partial class Manager
    {
        public int Id { get; set; }
        public int IdPerson { get; set; }
        public int IdDealershipSalon { get; set; }
        public string IdAspNetUser { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual DealershipSalon DealershipSalon { get; set; }
        public virtual Person Person { get; set; }
    }
}
