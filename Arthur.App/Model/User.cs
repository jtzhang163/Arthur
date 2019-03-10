using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.App.Model
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
        [Required, MaxLength(255)]
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

    }

}
