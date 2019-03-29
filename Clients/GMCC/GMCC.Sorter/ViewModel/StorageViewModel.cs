using Arthur.App;
using GMCC.Sorter.Data;
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

        public StorageViewModel(int id, int column, int floor, string name, string company)
        {
            this.Id = id;
            this.Column = column;
            this.Floor = floor;
            this.name = name;
            this.company = company;
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
        /// 静置时间(min)
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
    }
}
