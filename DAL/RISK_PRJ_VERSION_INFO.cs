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
    
    public partial class RISK_PRJ_VERSION_INFO
    {
        public RISK_PRJ_VERSION_INFO()
        {
            this.RISK_PRJ_INSTANCE = new HashSet<RISK_PRJ_INSTANCE>();
            this.RISK_TASK_INSTANCE = new HashSet<RISK_TASK_INSTANCE>();
        }
    
        public long SIM_VERSION_ID { get; set; }
        public Nullable<long> PRI_ID { get; set; }
        public string DESCRIPTION { get; set; }
        public Nullable<long> COUNT { get; set; }
        public Nullable<long> DURATION_MAX { get; set; }
        public Nullable<System.DateTime> SIM_STARTTIME { get; set; }
        public Nullable<System.DateTime> SIM_ENDTIME { get; set; }
        public Nullable<short> HAVE_RESOURCE { get; set; }
    
        public virtual ICollection<RISK_PRJ_INSTANCE> RISK_PRJ_INSTANCE { get; set; }
        public virtual ICollection<RISK_TASK_INSTANCE> RISK_TASK_INSTANCE { get; set; }
    }
}