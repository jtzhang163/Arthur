using Arthur;
using Arthur.App.Data;
using Arthur.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.App
{
    public class Account
    {
        //public static bool Register(string name, string password, out string msg)
        //{
        //    return Register(name, "", password, false, out msg);
        //}

        //public static bool Register(string name, string number, string password, bool isEnabled, out string msg)
        //{

        //    User user = new User();
        //    if (Context.UserContext.Users.Where(u => u.Name == name).Count() > 0)
        //    {
        //        msg = "系统中已存在用户：" + name;
        //        return false;
        //    }
        //    user.Name = name;
        //    user.Nickname = name;
        //    user.Number = number;
        //    user.Password = Base64.EncodeBase64(password);
        //    user.IsEnabled = isEnabled;
        //    user.RegisterTime = DateTime.Now;
        //    user.RoleId = Context.UserContext.Roles.Single(r => r.Name == "操作员").Id;
        //    user.ProfilePicture = "/Images/Profiles/001.jpg";
        //    Context.UserContext.Users.Add(user);
        //    Context.UserContext.SaveChanges();

        //    msg = string.Empty;
        //    return true;
        //}


        public static Result Login(string name, string password)
        {
            name = name.Trim();
            password = password.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                return new Result("请输入用户名！");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return new Result("请输入密码！");
            }

            var user = new User();

            var entityPassword = EncryptHelper.EncodeBase64(password);
            user = Context.AccountContext.Users.FirstOrDefault(u => u.Name == name && u.Password == entityPassword) ?? new User();
            user.LastLoginTime = DateTime.Now;
            user.LoginTimes++;
            Context.AccountContext.SaveChanges();

            if (!user.IsEnabled)
            {
                return new Result(user.Name + "尚未审核或已被禁用！");
            }

            Current.User = user;

            if (Current.User.Id > 0)
            {
                return Result.OK;
            }
            return new Result("用户名或密码错误！");
        }


        //public static bool Logout()
        //{
        //    OperationHelper.ShowTips(AppCurrent.User.Name + "成功注销");
        //    AppCurrent.User = new User(-1);
        //    return true;
        //}
    }
}
