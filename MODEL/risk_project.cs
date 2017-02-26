using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
        public class risk_project
        {
                private int prj_id;

                public int Prj_id
                {
                        get { return prj_id; }
                        set { prj_id = value; }
                }
                private string prj_name;

                public string Prj_name
                {
                        get { return prj_name; }
                        set { prj_name = value; }
                }
                private string prj_describe;

                public string Prj_describe
                {
                        get { return prj_describe; }
                        set { prj_describe = value; }
                }
                private DateTime prj_date;

                public DateTime Prj_date
                {
                        get { return prj_date; }
                        set { prj_date = value; }
                }
                private string prj_xml;

                public string Prj_xml
                {
                        get { return prj_xml; }
                        set { prj_xml = value; }
                }
        }
}
