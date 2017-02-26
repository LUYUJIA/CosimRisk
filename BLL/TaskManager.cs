using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
        public class TaskManager
        {
                #region 添加项目
                #endregion

                #region 更新项目
                public static bool updateTask(MODEL.risk_task task)
                {
                        try
                        {
                                if (CommonArugment.getTask.updateTask(task))
                                        return true;
                                return false;
                        } catch (System.Exception ex)
                        {
                                throw ex;
                        }
                       
                }
                #endregion

                #region 删除项目
                #endregion

                #region 查找项目
                #endregion
        }
}
