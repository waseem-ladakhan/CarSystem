//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CarSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class invoice
    {
        public int inoviceID { get; set; }
        public string UserName { get; set; }
        public string RegistrationId { get; set; }
        public Nullable<System.DateTime> startDate { get; set; }
        public Nullable<System.DateTime> bookingDate { get; set; }
        public Nullable<System.DateTime> ReturnDate { get; set; }
        public Nullable<decimal> RentPerDay { get; set; }
        public Nullable<int> TotalAmmount { get; set; }
        public Nullable<int> BookingID { get; set; }
    
        public virtual car car { get; set; }
        public virtual Registration Registration { get; set; }
    }
}