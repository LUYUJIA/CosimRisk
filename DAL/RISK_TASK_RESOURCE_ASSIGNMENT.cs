//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class RISK_TASK_RESOURCE_ASSIGNMENT
    {
        public long AUTO_ID { get; set; }
        public Nullable<long> TASK_AUTO_ID { get; set; }
        public Nullable<long> RESOURCE_ID { get; set; }
        public Nullable<long> ASSIGNMENT_AMOUNT { get; set; }
        public Nullable<long> ASSIGNMENT_OWN { get; set; }
    
        public virtual RISK_RESOURCE RISK_RESOURCE { get; set; }
        public virtual RISK_TASK RISK_TASK { get; set; }
    }
}
