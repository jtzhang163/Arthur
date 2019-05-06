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
        public TaskType Type { get; set; }

        public TaskStatus Status { get; set; }

        public int ProcTrayId { get; set; }

        public int StorageId { get; set; }

        public TaskType PreType { get; set; }

        public DateTime StartTime { get; set; } = Default.DateTime;
    }

    public enum TaskType
    {
        未知 = 0,
        上料 = 1,
        下料 = 2
    }
    public enum TaskStatus
    {
        未知 = 0,
        就绪 = 1,
        准备搬 = 2,
        搬运中 = 3,
        回位中 = 4,
        完成 = 5
    }
}
