﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CarProjectEntities : DbContext
    {
        public CarProjectEntities()
            : base("name=CarProjectEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<myBooking> myBookings { get; set; }
        public virtual DbSet<Registration> Registrations { get; set; }
        public virtual DbSet<car> cars { get; set; }
        public virtual DbSet<expectation> expectations { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<invoice> invoices { get; set; }
        public virtual DbSet<Driver> Drivers { get; set; }
    }
}
