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
    
    public partial class RISK_RESOURCE
    {
        public RISK_RESOURCE()
        {
            this.RISK_PROJECT_RES_ASSIGNMENT = new HashSet<RISK_PROJECT_RES_ASSIGNMENT>();
            this.RISK_TASK_RESOURCE_ASSIGNMENT = new HashSet<RISK_TASK_RESOURCE_ASSIGNMENT>();
        }
    
        public long AUTO_ID { get; set; }
        public string RESOURCE_NAME { get; set; }
        public long RESOURCE_TYPE { get; set; }
        public long RESOURCE_AMOUNT { get; set; }
        public Nullable<decimal> RESOURCE_UNIT_PRICE { get; set; }
        public string RESOURCE_DESCRIPTION { get; set; }
        public Nullable<long> RESOURCE_REMAINS { get; set; }
    
        public virtual ICollection<RISK_PROJECT_RES_ASSIGNMENT> RISK_PROJECT_RES_ASSIGNMENT { get; set; }
        public virtual RISK_RESOURCE_TYPE RISK_RESOURCE_TYPE { get; set; }
        public virtual ICollection<RISK_TASK_RESOURCE_ASSIGNMENT> RISK_TASK_RESOURCE_ASSIGNMENT { get; set; }
    }
}
