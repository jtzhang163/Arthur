using Arthur;
using Arthur.App;
using Arthur.App.Comm;
using Arthur.App.Model;
using GMCC.Sorter.Data;
using GMCC.Sorter.Model;
using GMCC.Sorter.Run;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMCC.Sorter.ViewModel
{
    /// <summary>
    /// 当前任务
    /// </summary>
    public class CurrentTaskViewModel : BindableObject
    {
        private TaskType taskType = TaskType.未知;
        private int procTrayId = -1;
        private int storageId = -1;
        private DateTime startTime = Default.DateTime;

        public CurrentTaskViewModel(CurrentTask task)
        {
            this.taskType = task.TaskType;
            this.procTrayId = task.ProcTrayId;
            this.storageId = task.StorageId;
            this.startTime = task.StartTime;
        }

        public TaskType TaskType
        {
            get => taskType;
            set
            {
                if (taskType != value)
                {
                    Context.CurrentTask.TaskType = value;
                    this.SetProperty(ref taskType, value);
                }
            }
        } 

        public int ProcTrayId
        {
            get => procTrayId;
            set
            {
                if (procTrayId != value)
                {
                    Context.CurrentTask.ProcTrayId = value;
                    this.SetProperty(ref procTrayId, value);
                }
            }
        }

        public int StorageId
        {
            get => storageId;
            set
            {
                if (storageId != value)
                {
                    Context.CurrentTask.StorageId = value;
                    this.SetProperty(ref storageId, value);
                }
            }
        }

        public DateTime StartTime
        {
            get => startTime;
            set
            {
                if (startTime != value)
                {
                    Context.CurrentTask.StartTime = value;
                    this.SetProperty(ref startTime, value);
                }
            }
        }

    }
}
