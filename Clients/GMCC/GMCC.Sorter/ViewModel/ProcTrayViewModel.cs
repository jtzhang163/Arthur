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
    /// 流程托盘
    /// </summary>
    public class ProcTrayViewModel : BindableObject
    {

        public int Id { get; set; }

        private string code = null;
        public string Code
        {
            get
            {
                return code;
            }
            set
            {
                if (code != value)
                {
                    var contextProcTray = Context.ProcTrays.FirstOrDefault(o => o.Id == this.Id);
                    contextProcTray.Code = value;
                    Arthur.Business.Logging.AddOplog(string.Format("数据追溯. 流程托盘. Id:{0} 条码: [{1}] 修改为 [{2}]", Id, code, value), Arthur.App.Model.OpType.编辑);
                    this.SetProperty(ref code, value);
                    Context.Trays.Where(o => o.Id == contextProcTray.TrayId).ToList().ForEach(o => o.Code = value);
                }
            }
        }

        private DateTime scanTime;
        public DateTime ScanTime
        {
            get
            {
                return scanTime;
            }
            set
            {
                if (scanTime != value)
                {
                    Context.ProcTrays.FirstOrDefault(o => o.Id == this.Id).ScanTime = value;
                    Arthur.Business.Logging.AddOplog(string.Format("数据追溯. 流程托盘. 条码:{0} 扫码时间: [{1}] 修改为 [{2}]", Code, scanTime, value), Arthur.App.Model.OpType.编辑);
                    this.SetProperty(ref scanTime, value);
                }
            }
        }

        private int storageId;
        public int StorageId
        {
            get
            {
                return storageId;
            }
            set
            {
                if (storageId != value)
                {
                    Context.ProcTrays.FirstOrDefault(o => o.Id == this.Id).StorageId = value;
                    Arthur.Business.Logging.AddOplog(string.Format("数据追溯. 流程托盘. 条码:{0} 料仓Id: [{1}] 修改为 [{2}]", Code, storageId, value), Arthur.App.Model.OpType.编辑);
                    this.SetProperty(ref storageId, value);

                    //同时更新Storage的ProcTrayId属性
                    Current.Storages.Where(o => o.ProcTrayId == this.Id).ToList().ForEach(o => o.ProcTrayId = -1);
                    Current.Storages.Where(o => o.Id == value).ToList().ForEach(o => o.ProcTrayId = this.Id);
                }
            }
        }

        private DateTime startStillTime;
        public DateTime StartStillTime
        {
            get
            {
                return startStillTime;
            }
            set
            {
                if (startStillTime != value)
                {
                    Context.ProcTrays.FirstOrDefault(o => o.Id == this.Id).StartStillTime = value;
                    Arthur.Business.Logging.AddOplog(string.Format("数据追溯. 流程托盘. 条码:{0} 开始静置时间: [{1}] 修改为 [{2}]", Code, startStillTime, value), Arthur.App.Model.OpType.编辑);
                    this.SetProperty(ref startStillTime, value);
                }
            }
        }

        private int stillTimeSpan = 0;
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
                    Context.ProcTrays.FirstOrDefault(o => o.Id == this.Id).StillTimeSpan = value;
                    this.SetProperty(ref stillTimeSpan, value);
                }
            }
        }


        public ProcTrayViewModel(int id, string code, DateTime scanTime, int storageId, DateTime startStillTime, int stillTimeSpan)
        {
            this.Id = id;
            this.code = code;
            this.scanTime = scanTime;
            this.storageId = storageId;
            this.startStillTime = startStillTime;
            this.StillTimeSpan = stillTimeSpan;
        }
    }
}
