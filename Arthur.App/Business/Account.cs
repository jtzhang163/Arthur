﻿using Arthur.Core;
using Arthur.App.Data;
using Arthur.Core.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arthur.App;
using Arthur.App.Model;
using Arthur.App.Utils;

namespace Arthur.App.Business
{
    public static class Account
    {
        public static Result Register(string name, string password, string confirm_pwd)
        {
            name = name.Trim();
            password = password.Trim();
            confirm_pwd = confirm_pwd.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                return new Result("请输入用户名！");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return new Result("请输入密码！");
            }

            if (string.IsNullOrWhiteSpace(confirm_pwd))
            {
                return new Result("再次输入密码！");
            }

            if (string.IsNullOrWhiteSpace(confirm_pwd))
            {
                return new Result("再次输入密码！");
            }

            if (password != confirm_pwd)
            {
                return new Result("两次输入密码不一致！");
            }

            return Register(name, "", password, false);
        }

        public static Result Register(string name, string number, string password, bool isEnabled)
        {
            User user = new User();
            try
            {
                using (var db = new Arthur.App.AppContext())
                {
                    if (db.Users.Where(u => u.Name == name).Count() > 0)
                    {
                        return new Result("系统中已存在用户：" + name);
                    }
                    user.Name = name;
                    user.Number = number;
                    user.Email = string.Empty;
                    user.Gender = Gender.Unknown;
                    user.Nickname = name;
                    user.PhoneNumber = string.Empty;
                    user.Password = EncryptBase64Helper.EncodeBase64(password);
                    user.IsEnabled = isEnabled;
                    user.RegisterTime = DateTime.Now;
                    user.RoleId = db.Roles.Single(r => r.Name == "操作员").Id;
                    db.Users.Add(user);
                    db.SaveChanges();
                }
                Arthur.App.Business.Logging.AddOplog(user.Id, string.Format("注册用户[{0}]", user.Name), App.Model.OpType.创建);
            }
            catch (Exception ex)
            {
                Arthur.App.Business.Logging.AddEvent(string.Format("注册用户[{0}]出错", user.Name), EventType.错误);
                LogHelper.WriteError(ex);
                return new Result(ex.Message);
            }

            return Result.Success;
        }

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

            var entityPassword = EncryptBase64Helper.EncodeBase64(password);

            var user = new User();

            try
            {
                using (var db = new Arthur.App.AppContext())
                {
                    user = db.Users.FirstOrDefault(u => u.Name == name && u.Password == entityPassword) ?? new User();

                    if (user.Id < 1)
                    {
                        return new Result("用户名或密码错误！");
                    }

                    if (!user.IsEnabled)
                    {
                        return new Result(user.Name + "尚未激活或被禁用！");
                    }

                    user.LastLoginTime = DateTime.Now;
                    user.LoginTimes++;

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Business.Logging.AddEvent(string.Format("登录时出现错误，{0}", ex.Message), EventType.错误);
                LogHelper.WriteError(ex);
                return new Result("登录失败，msg：" + ex.Message);
            }

            Current.User = ContextToViewModel.Convert(user);
            return Result.Success;
        }

        public static Result ChangePassword(string username, string old_pwd, string new_pwd, string confirm_new_pwd)
        {
            #region 输入校验
            if (string.IsNullOrWhiteSpace(username))
            {
                return new Result("请输入用户名！");
            }

            if (string.IsNullOrWhiteSpace(old_pwd))
            {
                return new Result("请输入原密码！");
            }

            if (string.IsNullOrWhiteSpace(new_pwd))
            {
                return new Result("请输入新密码！");
            }

            if (string.IsNullOrWhiteSpace(confirm_new_pwd))
            {
                return new Result("请再次输入新密码！");
            }

            if (new_pwd != confirm_new_pwd)
            {
                return new Result("两次输入的新密码不一致！");
            }

            if (old_pwd == new_pwd)
            {
                return new Result("输入的新密码和原密码不能相同！");
            }
            #endregion

            using (var db = new Arthur.App.AppContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Name == username) ?? new User();
                if (user.Id < 1)
                {
                    return new Result("不存在用户：" + username);
                }

                if (user.Password != EncryptBase64Helper.EncodeBase64(old_pwd))
                {
                    return new Result("原密码输入错误！");
                }

                user.Password = EncryptBase64Helper.EncodeBase64(new_pwd);
                db.SaveChanges();
            }

            return Result.Success;
        }

        public static IEnumerable<Role> GetRoles()
        {
            using (var db = new Arthur.App.AppContext())
            {
                return db.Roles.ToList();
            }
        }

    }
}
