using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public class risk_math_expression_arg
    {
        private int auto_id;

        public int Auto_id
        {
            get { return auto_id; }
            set { auto_id = value; }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private byte[] value;

        public byte[] Value
        {
            get { return this.value; }
            set { this.value = value; }
        }


        private int task_id;  //其实是指task的autoid

        public int Task_id
        {
            get { return task_id; }
            set { task_id = value; }
        }
        private int expression_id;

        public int Expression_id
        {
            get { return expression_id; }
            set { expression_id = value; }
        }

        private double actual_value;

        public double Actual_value
        {
            get { return actual_value; }
            set { actual_value = value; }
        }
    }
}
