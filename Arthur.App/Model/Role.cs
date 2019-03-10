using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.App
{
    /// <summary>
    /// 用户组别
    /// </summary>
    public class Role : Service
    {

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
        public int Level { get; set; }
        /// <summary>
        /// 用户组别名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 该用户组别下所有用户
        /// </summary>
        public virtual ICollection<User> Users { get; set; }
        #endregion
    }
}
