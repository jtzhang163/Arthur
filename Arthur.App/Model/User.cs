using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.App
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User : Service
    {

        /// <summary>
        /// 用户名称
        /// </summary>
        [Required, MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [MaxLength(50)]
        public string Nickname { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        [MaxLength(100)]
        public string ProfilePicture { get; set; }

        /// <summary>
        /// 用户登录密码
        /// </summary>
        [Required, MaxLength(100)]
        public string Password { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        [MaxLength(50)]
        public string Number { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [MaxLength(30)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        [MaxLength(30)]
        public string Email { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? RegisterTime { get; set; }

        /// <summary>
        /// 上次登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 登录次数
        /// </summary>
        public int LoginTimes { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// 所属类别Id
        /// </summary>
        public int RoleId { get; set; }

        // <summary>
        // 所属类别
        // </summary>
        public virtual Role Role { get; set; }

        public User() : this(-1)
        {
            Id = -1;
        }

        public User(int id)
        {
            Id = id;
        }


        public static bool Register(string name, string password, out string msg)
        {
            return Register(name, "", password, false, out msg);
        }

        public static bool Register(string name, string number, string password, bool isEnabled, out string msg)
        {

            User user = new User();
            if (Context.UserContext.Users.Where(u => u.Name == name).Count() > 0)
            {
                msg = "系统中已存在用户：" + name;
                return false;
            }
            user.Name = name;
            user.Nickname = name;
            user.Number = number;
            user.Password = Base64.EncodeBase64(password);
            user.IsEnabled = isEnabled;
            user.RegisterTime = DateTime.Now;
            user.RoleId = Context.UserContext.Roles.Single(r => r.Name == "操作员").Id;
            user.ProfilePicture = "/Images/Profiles/001.jpg";
            Context.UserContext.Users.Add(user);
            Context.UserContext.SaveChanges();

            msg = string.Empty;
            return true;
        }


        public static bool Login(string name, string password, out string msg)
        {
            var user = new User();


            var entityPassword = Base64.EncodeBase64(password);
            user = Context.UserContext.Users.FirstOrDefault(u => u.Name == name && u.Password == entityPassword) ?? new User();
            user.LastLoginTime = DateTime.Now;
            user.LoginTimes++;
            Context.UserContext.SaveChanges();

            if (!user.IsEnabled)
            {
                msg = user.Name + "尚未审核或已被禁用";
                return false;
            }

            AppCurrent.User = user;

            if (AppCurrent.User.Id > 0)
            {
                msg = string.Empty;
                return true;
            }
            msg = "用户名或密码错误";
            return false;
        }

        public static bool Logout()
        {
            OperationHelper.ShowTips(AppCurrent.User.Name + "成功注销");
            AppCurrent.User = new User(-1);
            return true;
        }

    }

    public class UserFactory
    {
        private ObservableCollection<User> users = new ObservableCollection<User>();

        public UserFactory()
        {
            Context.UserContext.Users.ToList().ForEach(u => users.Add(u));
        }

        public IEnumerable<User> GetUsers()
        {
            return users;
        }
    }

}
