using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.App.Model
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public class Role : Service
    {
        private int? _level;
        private string _name;

        public Role() : this(-1)
        {

        }

        public Role(int id)
        {
            Id = id;
        }


        #region 属性
        /// <summary>
        /// 等级
        /// </summary>
        public int Level
        {
            get => _level.Value;
            set
            {
                if (_level != null && _level != value && this.Id > 0)
                {
                    Arthur.Business.Logging.AddOplog(string.Format("角色[{2}]. 等级: [{0}] 修改为 [{1}]", _level, value, this.Name), Arthur.App.Model.OpType.编辑);
                }
                _level = value;
            }
        }
        /// <summary>
        /// 用户角色名称
        /// </summary>
        [MaxLength(50)]
        public string Name
        {
            get => _name;
            set
            {
                if (_name != null && _name != value && this.Id > 0)
                {
                    Arthur.Business.Logging.AddOplog(string.Format("角色. 名称: [{0}] 修改为 [{1}]", _level, value), Arthur.App.Model.OpType.编辑);
                }
                _name = value;
            }
        }

        /// <summary>
        /// 该用户组别下所有用户
        /// </summary>
        public virtual ICollection<User> Users { get; set; }
        #endregion
    }
}
