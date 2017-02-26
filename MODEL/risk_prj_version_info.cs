using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
        public class risk_prj_version_info
        {
                public risk_prj_version_info()
                {

                }
                public risk_prj_version_info(int prjId, string desciption, int times, DateTime startTime, DateTime endTime,int have_resource)
                {
                        this.pri_id = prjId;
                        this.desciption = desciption;
                        this.count = times;
                        this.sim_starttime = startTime;
                        this.sim_endtime = endTime;
                        this.have_resource = have_resource;
                }
                private int sim_version_id;

                public int Sim_version_id
                {
                        get { return sim_version_id; }
                        set { sim_version_id = value; }
                }
                private int pri_id;

                public int Pri_id
                {
                        get { return pri_id; }
                        set { pri_id = value; }
                }
                private string desciption;

                public string Desciption
                {
                        get { return desciption; }
                        set { desciption = value; }
                }
                private int count;

                public int Count
                {
                        get { return count; }
                        set { count = value; }
                }
                private int duration_max;

                public int Duration_max
                {
                        get { return duration_max; }
                        set { duration_max = value; }
                }
                private DateTime sim_starttime;

                public DateTime Sim_starttime
                {
                        get { return sim_starttime; }
                        set { sim_starttime = value; }
                }
                private DateTime sim_endtime;

                public DateTime Sim_endtime
                {
                        get { return sim_endtime; }
                        set { sim_endtime = value; }
                }
                private int have_resource;

                public int Have_resource
                {
                    get { return have_resource; }
                    set { have_resource = value; }
                }
                #region transient
                private string projectName;

                public string ProjectName
                {
                        get { return projectName; }
                        set { projectName = value; }
                }
                #endregion

        }
}
