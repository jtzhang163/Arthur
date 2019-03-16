using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arthur.App.Data;
using Arthur.App.Model;

namespace Arthur.Business
{
    public class RoleManage : IManage<Role>
    {

        public Result Create(Role role)
        {
            if (Context.Roles.Count(r => r.Name == role.Name) > 0)
            {
                return new Result(string.Format("系统中已存在名为{0}的角色！", role.Name));
            }
            try
            {
                Context.Roles.Add(new App.Model.Role() { Level = role.Level, Name = role.Name });
                Context.AccountContext.SaveChanges();
                return Result.OK;
            }
            catch (Exception ex)
            {
                return new Result(ex.Message);
            }

        }


    }
}
