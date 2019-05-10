using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.App.ViewModel
{
    public class RoleViewModel
    {


        public int Id { get; set; }
        private int? _level;
        private string _name;

        public RoleViewModel(int id, int? level, string name)
        {
            Id = id;
            _level = level;
            _name = name;
        }

        /// <summary>
        /// 等级
        /// </summary>
        public int Level
        {
            get => _level.Value;
            set
            {
                if (_level != value)
                {
                    using (var db = new Arthur.App.AppContext())
                    {
                        var role = db.Roles.FirstOrDefault(o => o.Id == this.Id);
                        if (role != null)
                        {
                            role.Level = value;
                            db.SaveChanges();
                        }
                    }
                    if (_level != null && this.Id > 0)
                    {
                        Arthur.Business.Logging.AddOplog(string.Format("角色[{2}]. 等级: [{0}] 修改为 [{1}]", _level, value, this.Name), Arthur.App.Model.OpType.编辑);
                    }
                    _level = value;
                }
            }
        }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if(_name != value)
                {
                    using (var db = new Arthur.App.AppContext())
                    {
                        var role = db.Roles.FirstOrDefault(o => o.Id == this.Id);
                        if (role != null)
                        {
                            role.Name = value;
                            db.SaveChanges();
                        }
                    }
                    if (_name != null && this.Id > 0)
                    {
                        Arthur.Business.Logging.AddOplog(string.Format("角色. 名称: [{0}] 修改为 [{1}]", _level, value), Arthur.App.Model.OpType.编辑);
                    }
                    _name = value;
                }
            }
        }
    }
}
