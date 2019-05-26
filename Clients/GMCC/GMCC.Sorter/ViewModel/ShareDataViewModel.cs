using Arthur.App;
using GMCC.Sorter.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.ViewModel
{
    public sealed class ShareDataViewModel : BindableObject
    {
        public int Id { get; set; }
        public string Key { get; set; }

        private string _value = null;
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value != value && _value != null)
                {
                    using (var db = new ShareContext())
                    {
                        var count = db.Database.ExecuteSqlCommand(string.Format("UPDATE dbo.t_data SET [Value] = '{0}' WHERE [Key] = '{1}'", value, this.Key));
                        if (count > 0)
                        {
                            Arthur.App.Business.Logging.AddOplog(string.Format("交互平台. BTS客户端. 键:{0} 值: [{1}] 修改为 [{2}]", Key, _value, value), Arthur.App.Model.OpType.编辑);
                        }
                    }
                }
                this.SetProperty(ref _value, value);
            }
        }

        private int _status = -1;
        public int Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (_status != value && _status != -1)
                {
                    using (var db = new ShareContext())
                    {
                        var count = db.Database.ExecuteSqlCommand(string.Format("UPDATE dbo.t_data SET [Status] = '{0}' WHERE [Key] = '{1}'", value, this.Key));
                        if (count > 0)
                        {
                            Arthur.App.Business.Logging.AddOplog(string.Format("交互平台. BTS客户端. 键:{0} 状态: [{1}] 修改为 [{2}]", Key, _status, value), Arthur.App.Model.OpType.编辑);
                        }
                    }
                }
                this.SetProperty(ref _status, value);
            }
        }

        private int _procTrayId = -1;
        public int ProcTrayId
        {
            get
            {
                return _procTrayId;
            }
            set
            {
                if (_procTrayId != value && _procTrayId != -1)
                {
                    using (var db = new ShareContext())
                    {
                        var count = db.Database.ExecuteSqlCommand(string.Format("UPDATE dbo.t_data SET [ProcTrayId] = {0} WHERE [Key] = '{1}'", value, this.Key));
                        if (count > 0)
                        {
                            Arthur.App.Business.Logging.AddOplog(string.Format("交互平台. BTS客户端. 键:{0} 流程托盘ID: [{1}] 修改为 [{2}]", Key, _procTrayId, value), Arthur.App.Model.OpType.编辑);
                        }
                    }
                }
                this.SetProperty(ref _procTrayId, value);
            }
        }

        private DateTime _updateTime = Arthur.Core.Default.DateTime;
        public DateTime UpdateTime
        {
            get
            {
                return _updateTime;
            }
            set
            {
                if (_updateTime != value && _updateTime != Arthur.Core.Default.DateTime)
                {
                    using (var db = new ShareContext())
                    {
                        var count = db.Database.ExecuteSqlCommand(string.Format("UPDATE dbo.t_data SET [UpdateTime] = '{0}' WHERE [Key] = '{1}'", value, this.Key));
                        if (count > 0)
                        {
                            Arthur.App.Business.Logging.AddOplog(string.Format("交互平台. BTS客户端. 键:{0} 更新时间: [{1}] 修改为 [{2}]", Key, _updateTime, value), Arthur.App.Model.OpType.编辑);
                        }
                    }
                }
                this.SetProperty(ref _updateTime, value);
            }
        }

        public string Desc { get; set; }

        public ShareDataViewModel(int id, string key, string value, int status, int procTrayId, DateTime updateTime, string desc)
        {
            this.Id = id;
            this.Key = key;
            this.Value = value;
            this.Status = status;
            this.ProcTrayId = procTrayId;
            this.UpdateTime = updateTime;
            this.Desc = desc;
        }
    }
}
