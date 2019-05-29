using Arthur.Core;
using Arthur.App.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arthur.App.Data;
using Arthur.App.Model;

namespace Arthur.App.Business
{
    public sealed class RoleManage : IManage<Role>
    {

        public Result Create(Role role)
        {
            try
            {
                using (var db = new Arthur.App.AppContext())
                {
                    if (db.Roles.Count(r => r.Name == role.Name) > 0)
                    {
                        return new Result(string.Format("系统中已存在名为{0}的角色！", role.Name));
                    }
                    db.Roles.Add(new App.Model.Role() { Level = role.Level, Name = role.Name });
                    db.SaveChanges();
                }
                Arthur.App.Business.Logging.AddOplog(string.Format("新增角色[{0}]", role.Name), App.Model.OpType.创建);
                return Result.Success;
            }
            catch (Exception ex)
            {
                return new Result(ex.Message);
            }
        }
    }
}
