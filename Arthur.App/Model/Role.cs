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
    public class Role
    {

        public Role() : this(-1)
        {

        }

        public Role(int id)
        {
            Id = id;
        }


        #region 属性
        public int Id { get; set; }
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


    public class RoleFactory
    {
        private ObservableCollection<Role> roles = new ObservableCollection<Role>();

        public RoleFactory()
        {
            Context.UserContext.Roles.ToList().ForEach(r => roles.Add(r));
        }

        public IEnumerable<Role> GetRoles()
        {
            return roles;
        }

        public IEnumerable<Role> GetLowAuthorityRoles()
        {
            return roles.Where(r => r.Level <= AppCurrent.User.Role.Level);
        }
    }
}
