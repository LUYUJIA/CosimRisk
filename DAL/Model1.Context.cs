﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<RISK_LINK> RISK_LINK { get; set; }
        public DbSet<RISK_LINK_TYPE> RISK_LINK_TYPE { get; set; }
        public DbSet<RISK_MATH_EXPRESSION> RISK_MATH_EXPRESSION { get; set; }
        public DbSet<RISK_MATH_EXPRESSION_ARG> RISK_MATH_EXPRESSION_ARG { get; set; }
        public DbSet<RISK_PRJ_INSTANCE> RISK_PRJ_INSTANCE { get; set; }
        public DbSet<RISK_PRJ_VERSION_INFO> RISK_PRJ_VERSION_INFO { get; set; }
        public DbSet<RISK_PROJECT> RISK_PROJECT { get; set; }
        public DbSet<RISK_PROJECT_RES_ASSIGNMENT> RISK_PROJECT_RES_ASSIGNMENT { get; set; }
        public DbSet<RISK_RESOURCE> RISK_RESOURCE { get; set; }
        public DbSet<RISK_RESOURCE_TYPE> RISK_RESOURCE_TYPE { get; set; }
        public DbSet<RISK_TASK> RISK_TASK { get; set; }
        public DbSet<RISK_TASK_INSTANCE> RISK_TASK_INSTANCE { get; set; }
        public DbSet<RISK_TASK_RESOURCE_ASSIGNMENT> RISK_TASK_RESOURCE_ASSIGNMENT { get; set; }
        public DbSet<RISK_PRJ_INSTANCE_RES> RISK_PRJ_INSTANCE_RES { get; set; }
        public DbSet<RISK_TASK_INSTANCE_RES> RISK_TASK_INSTANCE_RES { get; set; }
    }
}