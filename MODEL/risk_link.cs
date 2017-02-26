using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    [Serializable]
    public class risk_link : ICloneable
    {
        private int auto_id;

        public int Auto_id
        {
            get { return auto_id; }
            set { auto_id = value; }
        }
        private int prj_id;

        public int Prj_id
        {
            get { return prj_id; }
            set { prj_id = value; }
        }
        private int task_pre_id;

        public int Task_pre_id
        {
            get { return task_pre_id; }
            set { task_pre_id = value; }
        }
        private int task_suc_id;

        public int Task_suc_id
        {
            get { return task_suc_id; }
            set { task_suc_id = value; }
        }
        private int link_type;

        public int Link_type
        {
            get { return link_type; }
            set { link_type = value; }
        }
        private int delay_days;

        private string link_type_name;
        public string Link_type_name
        {
            get { return link_type_name; }
            set { link_type_name = value; }
        }

        public int Delay_days
        {
            get { return delay_days; }
            set { delay_days = value; }
        }

        public object Clone()
        {
            //创建内存流     
            MemoryStream ms = new MemoryStream();
            //以二进制格式进行序列化          
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, this);
            //反序列化当前实例到一个object    
            ms.Seek(0, 0);
            object obj = bf.Deserialize(ms);
            //关闭内存流            
            ms.Close();
            return obj;
        }

    }
}
