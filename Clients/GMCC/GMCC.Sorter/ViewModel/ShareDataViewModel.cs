using Arthur.App;
using GMCC.Sorter.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.ViewModel
{
    public class ShareDataViewModel : BindableObject
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
                            Arthur.Business.Logging.AddOplog(string.Format("交互平台. BTS客户端. 键:{0} 值: [{1}] 修改为 [{2}]", Key, _value, value), Arthur.App.Model.OpType.编辑);
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
                            Arthur.Business.Logging.AddOplog(string.Format("交互平台. BTS客户端. 键:{0} 状态: [{1}] 修改为 [{2}]", Key, _status, value), Arthur.App.Model.OpType.编辑);
                        }
                    }
                }
                this.SetProperty(ref _status, value);
            }
        }

        public string Desc { get; set; }

        public ShareDataViewModel(int id, string key, string value, int status, string desc)
        {
            this.Id = id;
            this.Key = key;
            this.Value = value;
            this.Status = status;
            this.Desc = desc;
        }
    }
}
