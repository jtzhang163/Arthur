using Arthur;
using Arthur.App;
using Arthur.App.Comm;
using Arthur.App.Model;
using GMCC.Sorter.Data;
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
        private Model.TaskType type = Model.TaskType.未知;
        private Model.TaskType preType = Model.TaskType.未知;
        private Model.TaskStatus status = Model.TaskStatus.未知;
        private int procTrayId = -1;
        private int storageId = -1;
        private DateTime startTime = Default.DateTime;

        public CurrentTaskViewModel(Model.CurrentTask task)
        {
            this.type = task.Type;
            this.preType = task.PreType;
            this.status = task.Status;
            this.procTrayId = task.ProcTrayId;
            this.storageId = task.StorageId;
            this.startTime = task.StartTime;
        }

        public Model.TaskType Type
        {
            get => type;
            set
            {
                if (type != value)
                {
                    Context.CurrentTask.Type = value;
                    this.SetProperty(ref type, value);
                }
            }
        }

        public Model.TaskType PreType
        {
            get => preType;
            set
            {
                if (preType != value)
                {
                    Context.CurrentTask.PreType = value;
                    this.SetProperty(ref preType, value);
                }
            }
        }

        public Model.TaskStatus Status
        {
            get => status;
            set
            {
                if (status != value)
                {
                    Context.CurrentTask.Status = value;
                    this.SetProperty(ref status, value);
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
