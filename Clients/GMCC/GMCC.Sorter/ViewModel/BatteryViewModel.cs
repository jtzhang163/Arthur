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
    /// 电池
    /// </summary>
    public sealed class BatteryViewModel : BindableObject
    {

        public int Id { get; set; }

        private string code;
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
                    using (var db = new Data.AppContext())
                    {
                        db.Batteries.FirstOrDefault(o => o.Id == this.Id).Code = value;
                        db.SaveChanges();
                    }
                    Arthur.Business.Logging.AddOplog(string.Format("数据追溯. 电池. Id:{0} 条码: [{1}] 修改为 [{2}]", Id, code, value), Arthur.App.Model.OpType.编辑);
                    this.SetProperty(ref code, value);
                }
            }
        }

        public DateTime ScanTime { get; set; }

        public int StorageId { get; set; }

        public DateTime StartStillTime { get; set; }

        private int pos;
        public int Pos
        {
            get
            {
                return pos;
            }
            set
            {
                if (pos != value)
                {
                    using (var db = new Data.AppContext())
                    {
                        db.Batteries.FirstOrDefault(o => o.Id == this.Id).Pos = value;
                        db.SaveChanges();
                    }
                    if (pos > 0)
                    {
                        Arthur.Business.Logging.AddOplog(string.Format("数据追溯. 电池. 条码:{0} 位置: [{1}] 修改为 [{2}]", Code, pos, value), Arthur.App.Model.OpType.编辑);
                    }
                    this.SetProperty(ref pos, value);
                }
            }
        }

        private int procTrayId = -2;

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
                        db.Batteries.FirstOrDefault(o => o.Id == this.Id).ProcTrayId = value;
                        db.SaveChanges();
                    }
                    if (procTrayId > -2)
                    {
                        Arthur.Business.Logging.AddOplog(string.Format("数据追溯. 电池. 条码:{0} ProcTrayId: [{1}] 修改为 [{2}]", Code, procTrayId, value), Arthur.App.Model.OpType.编辑);
                    }
                    this.SetProperty(ref procTrayId, value);
                }
            }
        }

        /// <summary>
        /// 静置时长(min)
        /// </summary>
        public int StillTimeSpan { get; set; }


        private SortResult sortResult = SortResult.Unknown;
        public SortResult SortResult
        {
            get
            {
                return sortResult;
            }
            set
            {
                if (sortResult != value)
                {
                    using (var db = new Data.AppContext())
                    {
                        db.Batteries.FirstOrDefault(o => o.Id == this.Id).SortResult = value;
                        db.SaveChanges();
                    }
                    this.SetProperty(ref sortResult, value);
                }
            }
        }

        public BatteryViewModel(int id, string code, int pos, DateTime scanTime, int storageId, int procTrayId, DateTime startStillTime, int stillTimeSpan, SortResult sortResult)
        {
            this.Id = id;
            this.code = code;
            this.Pos = pos;
            this.ScanTime = scanTime;
            this.StorageId = storageId;
            this.ProcTrayId = procTrayId;
            this.StartStillTime = startStillTime;
            this.StillTimeSpan = stillTimeSpan;
            this.SortResult = sortResult;
        }
    }
}
