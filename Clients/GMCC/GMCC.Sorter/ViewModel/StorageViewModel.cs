using Arthur.App;
using GMCC.Sorter.Data;
using GMCC.Sorter.Model;
using GMCC.Sorter.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.ViewModel
{
    public class StorageViewModel : BindableObject
    {

        public int Id { get; set; }

        public int Column { get; set; }

        public int Floor { get; set; }

        public StorageViewModel()
        {

        }

        public StorageViewModel(int id, int column, int floor, string name, string company, int stillTimeSpan, int procTrayId)
        {
            this.Id = id;
            this.Column = column;
            this.Floor = floor;
            this.name = name;
            this.company = company;
            this.stillTimeSpan = stillTimeSpan;
            this.procTrayId = procTrayId;
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
                    Context.Storages.FirstOrDefault(o => o.Id == this.Id).Name = value;
                    Arthur.Business.Logging.AddOplog(string.Format("设备管理. [{0}] 名称修改为 [{1}]", name, value), Arthur.App.Model.OpType.编辑);
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
                    Context.Storages.FirstOrDefault(o => o.Id == this.Id).Company = value;
                    Arthur.Business.Logging.AddOplog(string.Format("设备管理. {0} 品牌: [{1}] 修改为 [{2}]", Name, company, value), Arthur.App.Model.OpType.编辑);
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
                    Context.Storages.FirstOrDefault(o => o.Id == this.Id).StillTimeSpan = value;
                    Arthur.Business.Logging.AddOplog(string.Format("设备管理. {0} 静置时间: [{1}] 修改为 [{2}]", Name, stillTimeSpan, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref stillTimeSpan, value);
                }
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
                    var storage = Context.Storages.FirstOrDefault(o => o.Id == this.Id);
                    if (storage != null)
                    {
                        storage.ProcTrayId = value;
                        if(value > 0)
                        {
                            GetObject.GetById<ProcTray>(value).StorageId = storage.Id;
                        }
                    }

                    Arthur.Business.Logging.AddOplog(string.Format("设备管理. {0} 流程托盘Id: [{1}] 修改为 [{2}]", Name, procTrayId, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref procTrayId, value);
                    procTray = null;
                }
            }
        }

        private ProcTray procTray = null;

        public ProcTray ProcTray
        {
            get
            {
                if (procTray == null)
                {
                    procTray = Context.ProcTrays.FirstOrDefault(o => o.Id == this.ProcTrayId)?? new ProcTray();
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
    }

    public enum StorageStatus
    {
        未知 = 0,
        无托盘 = 1,
        正在静置 = 2,
        静置完成 = 3
    }
}
