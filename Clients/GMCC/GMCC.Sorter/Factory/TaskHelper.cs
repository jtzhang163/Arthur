using GMCC.Sorter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Factory
{
    public class TaskHelper
    {
        /// <summary>
        /// 任务类型（已排序）
        /// </summary>
        public static List<TaskType> TaskTypes
        {
            get
            {
                var types = new List<TaskType>();
                if (Current.Task.PreType == TaskType.上料)
                {
                    types.AddRange(new TaskType[] { TaskType.下料, TaskType.上料 });
                }
                else
                {
                    types.AddRange(new TaskType[] { TaskType.上料, TaskType.下料 });
                }
                return types;
            }
        }
    }
}
