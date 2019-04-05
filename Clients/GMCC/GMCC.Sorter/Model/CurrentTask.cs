using Arthur;
using Arthur.App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Model
{
    /// <summary>
    /// 当前任务
    /// </summary>
    public class CurrentTask : Service
    {
        public TaskType TaskType { get; set; }

        public int ProcTrayId { get; set; }

        public int StorageId { get; set; }

        public TaskType PreTaskType { get; set; }

        public DateTime StartTime { get; set; } = Default.DateTime;
    }

    public enum TaskType
    {
        未知 = 0,
        上料 = 1,
        下料 = 2
    }
}
