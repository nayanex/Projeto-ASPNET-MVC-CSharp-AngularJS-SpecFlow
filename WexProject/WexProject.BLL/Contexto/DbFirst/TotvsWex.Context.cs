﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WexProject.BLL.Contexto.DbFirst
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TotvsWexEntities : DbContext
    {
        public TotvsWexEntities()
            : base("name=TotvsWexEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<sysdiagrams> sysdiagrams { get; set; }
        public DbSet<BCHORAS> BCHORAS { get; set; }
        public DbSet<EVENTOSRH> EVENTOSRH { get; set; }
        public DbSet<RUBRICA> RUBRICA { get; set; }
    }
}