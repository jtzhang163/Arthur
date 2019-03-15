using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arthur.App.Data;
using Arthur.App.Model;

namespace Arthur.Business
{
    public class UserManage : IManage<User>
    {

        public Result Create(User user)
        {
            if (Context.AccountContext.Users.Count(r => r.Name == user.Name) > 0)
            {
                return new Result(string.Format("系统中已存在名为{0}的角色！", user.Name));
            }
            try
            {
                Context.AccountContext.Users.Add(new App.Model.User() { Name = user.Name });
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
