using Arthur.Core;
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
    public sealed class CurrentTaskViewModel : BindableObject
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
                    using (var db = new Data.AppContext())
                    {
                        db.CurrentTasks.FirstOrDefault().Type = value;
                        db.SaveChanges();
                    }
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
                    using (var db = new Data.AppContext())
                    {
                        db.CurrentTasks.FirstOrDefault().PreType = value;
                        db.SaveChanges();
                    }
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
                    using (var db = new Data.AppContext())
                    {
                        db.CurrentTasks.FirstOrDefault().Status = value;
                        db.SaveChanges();
                    }
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
                    using (var db = new Data.AppContext())
                    {
                        db.CurrentTasks.FirstOrDefault().ProcTrayId = value;
                        db.SaveChanges();
                    }
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
                    using (var db = new Data.AppContext())
                    {
                        db.CurrentTasks.FirstOrDefault().StorageId = value;
                        db.SaveChanges();
                    }
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
                    using (var db = new Data.AppContext())
                    {
                        db.CurrentTasks.FirstOrDefault().StartTime = value;
                        db.SaveChanges();
                    }
                    this.SetProperty(ref startTime, value);
                }
            }
        }

    }
}
