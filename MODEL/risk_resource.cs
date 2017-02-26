using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public class risk_resource
    {
        private int auto_id;

        public int Auto_id
        {
            get { return auto_id; }
            set { auto_id = value; }
        }

        private string resource_name;

        public string Resource_name
        {
            get { return resource_name; }
            set { resource_name = value; }
        }
        private string resource_type;

        public string Resource_type
        {
            get { return resource_type; }
            set { resource_type = value; }
        }
        private int resource_amount;

        public int Resource_amount
        {
            get { return resource_amount; }
            set { resource_amount = value; }
        }

        private int resource_remains;

        public int Resource_remains
        {
            get { return resource_remains; }
            set { resource_remains = value; }
        }
        private double resource_unit_price;

        public double Resource_unit_price
        {
            get { return resource_unit_price; }
            set { resource_unit_price = value; }
        }
        private string resource_description;

        public string Resource_description
        {
            get { return resource_description; }
            set { resource_description = value; }
        }

    }
}
