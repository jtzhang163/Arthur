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
        private Gender _gender;
        private string _number;
        private string _phoneNumber;
        private string _email;
        private bool? _isEnabled;
        private Role _role;

        /// <summary>
        /// 用户名称
        /// </summary>
        [Required, MaxLength(50)]
        public string Name { get; set; }

        public Gender Gender
        {
            get => _gender;
            set
            {
                if (_gender != value && this.Id > 0)
                {
                    Arthur.Business.Logging.AddOplog(string.Format("用户[{2}]. 性别: [{0}] 修改为 [{1}]", _gender, value, this.Name), Arthur.App.Model.OpType.编辑);
                }
                _gender = value;
            }
        }

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
        /// 工号
        /// </summary>
        [MaxLength(50)]
        public string Number
        {
            get => _number;
            set
            {
                if (_number != null && _number != value && this.Id > 0)
                {
                    Arthur.Business.Logging.AddOplog(string.Format("用户[{2}]. 工号: [{0}] 修改为 [{1}]", _number, value, this.Name), Arthur.App.Model.OpType.编辑);
                }
                _number = value;
            }
        }

        /// <summary>
        /// 手机号
        /// </summary>
        [MaxLength(30)]
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (_phoneNumber != null && _phoneNumber != value && this.Id > 0)
                {
                    Arthur.Business.Logging.AddOplog(string.Format("用户[{2}]. 手机号: [{0}] 修改为 [{1}]", _phoneNumber, value, this.Name), Arthur.App.Model.OpType.编辑);
                }
                _phoneNumber = value;
            }
        }

        /// <summary>
        /// 邮箱
        /// </summary>
        [MaxLength(30)]
        public string Email
        {
            get => _email;
            set
            {
                if (_email != null && _email != value && this.Id > 0)
                {
                    Arthur.Business.Logging.AddOplog(string.Format("用户[{2}]. 邮箱: [{0}] 修改为 [{1}]", _email, value, this.Name), Arthur.App.Model.OpType.编辑);
                }
                _email = value;
            }
        }

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
        public bool IsEnabled
        {
            get => _isEnabled.Value;
            set
            {
                if (_isEnabled != null && _isEnabled != value && this.Id > 0)
                {
                    Arthur.Business.Logging.AddOplog(string.Format("用户[{2}]. 是否启用: [{0}] 修改为 [{1}]", _isEnabled, value, this.Name), Arthur.App.Model.OpType.编辑);
                }
                _isEnabled = value;
            }
        }

        /// <summary>
        /// 所属类别Id
        /// </summary>
        public int RoleId { get; set; }

        // <summary>
        // 所属类别
        // </summary>
        public virtual Role Role
        {
            get => _role;
            set
            {
                if (_role != value && _role != null && value != null && this.Id > 0)
                {
                    Arthur.Business.Logging.AddOplog(string.Format("用户[{2}]. 角色: [{0}] 修改为 [{1}]", _role.Name, value.Name, this.Name), Arthur.App.Model.OpType.编辑);
                }
                _role = value;
            }
        }

        public User() : this(-1)
        {
            Id = -1;
        }

        public User(int id)
        {
            Id = id;
        }

    }

    public enum Gender
    {
        Unknown = 0,
        Male = 1,
        Female = 2
    }
}
