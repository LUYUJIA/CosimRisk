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
    
    public partial class RISK_MATH_EXPRESSION_ARG
    {
        public long AUTO_ID { get; set; }
        public string NAME { get; set; }
        public long TASK_AUTO_ID { get; set; }
        public long EXPRESSION_ID { get; set; }
        public byte[] VALUE { get; set; }
        public Nullable<decimal> ACTUAL_VALUE { get; set; }
    }
}
