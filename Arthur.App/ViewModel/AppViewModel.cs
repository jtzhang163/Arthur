using Arthur.App;
using Arthur.Utility;
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
                    SetProperty(ref dataGridPageSize, value);
                }
            }
        }
    }
}
