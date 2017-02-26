using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
        public class risk_task_instance
        {


                public risk_task_instance() { }
                public risk_task_instance(int taskAutoId, int prjId, double value, int sequence, int versionId)
                {
                        this.Task_auto_id = taskAutoId;
                        this.Prj_id = prjId;
                        this.Task_actual_dur_period = value;
                        this.Sim_sequence = sequence;
                        this.Sim_version = versionId;
                }
                private int auto_id;

                public int Auto_id
                {
                        get { return auto_id; }
                        set { auto_id = value; }
                }
                private int task_auto_id;

                public int Task_auto_id
                {
                        get { return task_auto_id; }
                        set { task_auto_id = value; }
                }


                private int prj_id;

                public int Prj_id
                {
                        get { return prj_id; }
                        set { prj_id = value; }
                }
                private int task_is_critical;

                public int Task_is_critical
                {
                        get { return task_is_critical; }
                        set { task_is_critical = value; }
                }
                private double task_actual_dur_period;

                public double Task_actual_dur_period
                {
                        get { return task_actual_dur_period; }
                        set { task_actual_dur_period = value; }
                }
                private double task_ve;

                public double Task_ve
                {
                        get { return task_ve; }
                        set { task_ve = value; }
                }
                private double task_vl;

                public double Task_vl
                {
                        get { return task_vl; }
                        set { task_vl = value; }
                }
                private int sim_sequence;

                public int Sim_sequence
                {
                        get { return sim_sequence; }
                        set { sim_sequence = value; }
                }
                private int sim_version;

                public int Sim_version
                {
                        get { return sim_version; }
                        set { sim_version = value; }
                }

                private double starttime = -1;

                public double Starttime
                {
                    get { return starttime; }
                    set { starttime = value; }
                
                }

                #region  transient
                private double averageProjectTime;

                public double AverageProjectTime
                {
                        get { return averageProjectTime; }
                        set { averageProjectTime = value; }
                }

                private double criticalRatio;

                public double CriticalRatio
                {
                        get { return criticalRatio; }
                        set { criticalRatio = value; }
                }
                #endregion
        }
}
