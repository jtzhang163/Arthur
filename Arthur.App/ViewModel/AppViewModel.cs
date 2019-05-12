using Arthur.App;
using Arthur.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.ViewModel
{
    public class AppViewModel : BindableObject
    {

        private string userName = null;

        public string UserName
        {
            get
            {
                if (userName == null)
                {
                    userName = Current.User.Name;
                }
                return userName;
            }
        }

        private int dataGridPageSize = -1;

        public int DataGridPageSize
        {
            get
            {
                if (dataGridPageSize < 0)
                {
                    dataGridPageSize = Current.Option.DataGridPageSize;
                }
                return dataGridPageSize;
            }
            set
            {
                if (dataGridPageSize != value)
                {
                    Current.Option.DataGridPageSize = value;
                    Arthur.Business.Logging.AddOplog(string.Format("系统参数. 表格每页显示数: [{0}] 修改为 [{1}]", dataGridPageSize, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref dataGridPageSize, value);
                }
            }
        }
    }
}
