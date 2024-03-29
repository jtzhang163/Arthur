﻿using Arthur.App;
using Arthur.Core;
using GMCC.Sorter.Data;
using GMCC.Sorter.Model;
using GMCC.Sorter.Other;
using GMCC.Sorter.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.ViewModel
{
    public sealed class StorageViewModel : BindableObject
    {

        public int Id { get; set; }

        public int Column { get; set; }

        public int Floor { get; set; }

        public StorageViewModel()
        {

        }

        public StorageViewModel(int id, int column, int floor, string name, string company, int stillTimeSpan, int procTrayId, bool isEnabled)
        {
            this.Id = id;
            this.Column = column;
            this.Floor = floor;
            this.name = name;
            this.company = company;
            this.stillTimeSpan = stillTimeSpan;
            this.procTrayId = procTrayId;
            this.isEnabled = isEnabled;
        }

        private string showInfo;
        public string ShowInfo
        {
            get
            {
                return showInfo;
            }
            set
            {
                if (showInfo != value)
                {
                    this.SetProperty(ref showInfo, value);
                }
            }
        }

        private string name = null;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    using (var db = new Data.AppContext())
                    {
                        db.Storages.FirstOrDefault(o => o.Id == this.Id).Name = value;
                        db.SaveChanges();
                    }
                    Arthur.App.Business.Logging.AddOplog(string.Format("设备管理. [{0}] 名称修改为 [{1}]", name, value), Arthur.App.Model.OpType.编辑);
                    this.SetProperty(ref name, value);
                }
            }
        }

        private string company = null;

        public string Company
        {
            get
            {
                return company;
            }
            set
            {
                if (company != value)
                {
                    using (var db = new Data.AppContext())
                    {
                        db.Storages.FirstOrDefault(o => o.Id == this.Id).Company = value;
                        db.SaveChanges();
                    }
                    Arthur.App.Business.Logging.AddOplog(string.Format("设备管理. {0} 品牌: [{1}] 修改为 [{2}]", Name, company, value), Arthur.App.Model.OpType.编辑);
                    this.SetProperty(ref company, value);
                }
            }
        }

        private int stillTimeSpan = -2;
        /// <summary>
        /// 静置时长(min)
        /// </summary>
        public int StillTimeSpan
        {
            get
            {
                return stillTimeSpan;
            }
            set
            {
                if (stillTimeSpan != value)
                {
                    using (var db = new Data.AppContext())
                    {
                        db.Storages.FirstOrDefault(o => o.Id == this.Id).StillTimeSpan = value;
                        db.SaveChanges();
                    }
                    Arthur.App.Business.Logging.AddOplog(string.Format("设备管理. {0} 静置时间: [{1}] 修改为 [{2}]", Name, stillTimeSpan, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref stillTimeSpan, value);
                }
            }
        }



        private bool? isEnabled;
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                if (this.Floor != 1)
                {
                    isEnabled = Current.Storages.FirstOrDefault(o => o.Column == this.Column && o.Floor == 1).IsEnabled;
                }
                return isEnabled.Value;
            }
            set
            {
                //确保每层设置一次，且不出现死循环
                var storage = Current.Storages.FirstOrDefault(o => o.Column == this.Column && o.Floor == this.Floor - 1);
                if (storage != null)
                {
                    storage.IsEnabled = value;
                }

                if (this.Floor == 1 && isEnabled != value && isEnabled.HasValue)
                {
                    using (var db = new Data.AppContext())
                    {
                        db.Storages.FirstOrDefault(o => o.Id == this.Id).IsEnabled = value;
                        db.SaveChanges();
                    }
                    Arthur.App.Business.Logging.AddOplog(string.Format("设备管理. {0} 是否启用: [{1}] 修改为 [{2}]", Name, isEnabled, value), Arthur.App.Model.OpType.编辑);
                }

                SetProperty(ref isEnabled, value);
            }
        }


        private int procTrayId = -2;
        /// <summary>
        /// 流程托盘Id
        /// </summary>
        public int ProcTrayId
        {
            get
            {
                return procTrayId;
            }
            set
            {
                if (procTrayId != value)
                {
                    using (var db = new Data.AppContext())
                    {
                        var storage = db.Storages.FirstOrDefault(o => o.Id == this.Id);
                        if (storage != null)
                        {
                            storage.ProcTrayId = value;
                            if (value > 0)
                            {
                                var procTray = db.ProcTrays.FirstOrDefault(o => o.Id == value);
                                if (procTray != null)
                                {
                                    procTray.StorageId = this.Id;
                                }
                            }
                            db.SaveChanges();
                        }
                    }

                    Arthur.App.Business.Logging.AddOplog(string.Format("设备管理. {0} 流程托盘Id: [{1}] 修改为 [{2}]", Name, procTrayId, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref procTrayId, value);
                    procTray = null; //很重要
                }
            }
        }

        private ProcTrayViewModel procTray = null;

        public ProcTrayViewModel ProcTray
        {
            get
            {
                if (procTray == null)
                {
                    using (var db = new Data.AppContext())
                    {
                        procTray = ContextToViewModel.Convert(db.ProcTrays.FirstOrDefault(o => o.Id == this.ProcTrayId) ?? new ProcTray());
                    }
                }
                return procTray;
            }
        }

        private StorageStatus status = StorageStatus.未知;
        public StorageStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                SetProperty(ref status, value);
            }
        }

        private TaskType taskType = TaskType.未知;
        public TaskType TaskType
        {
            get
            {
                return taskType;
            }
            set
            {
                SetProperty(ref taskType, value);
            }
        }

        private DateTime timeNow;
        public DateTime TimeNow
        {
            get
            {
                return timeNow;
            }
            set
            {
                SetProperty(ref timeNow, value);
            }
        }

        /// <summary>
        /// 获取料仓任务优先级
        /// </summary>
        /// <param name="taskType"></param>
        /// <param name="taskPriorityType"></param>
        /// <returns></returns>
        public long GetPriority(TaskType taskType, TaskPriorityType taskPriorityType)
        {
            if (taskType == TaskType.下料)
            {
                //最先开始静置的先下料
                return TimeHelper.GetTimeStamp(this.ProcTray.StartStillTime);
            }

            if (Current.Option.TaskPriorityType == Other.TaskPriorityType.层优先)
            {
                return (this.Column > Current.Option.LastFeedTaskStorageColumn ? 0 : 100) - this.Floor;
            }
            else
            {
                //比当前低一层的料仓
                var storageLower = Current.Storages.FirstOrDefault(o => o.Column == this.Column && o.Floor == this.Floor + 1);
                if (storageLower != null && storageLower.Status == StorageStatus.静置完成)
                {
                    //若某一料仓已完成静置，则比它高一层的料仓的上料优先级变低
                    return (this.Column > Current.Option.LastFeedTaskStorageColumn ? 0 : 100) + this.Column;
                }

                return (this.Column >= Current.Option.LastFeedTaskStorageColumn ? 0 : 100) + this.Column;
            }
        }
    }

    public enum StorageStatus
    {
        未知 = 0,
        无托盘 = 1,
        正在静置 = 2,
        静置完成 = 3
    }
}
