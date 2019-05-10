using Arthur.App.Model;
using Arthur.App.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.App.ViewModel
{
    public class UserViewModel
    {

        private Gender _gender;
        private string _number;
        private string _phoneNumber;
        private string _email;
        private bool? _isEnabled;
        private int _roleId;
        private RoleViewModel _role;

        public UserViewModel(int id, string name ,Gender gender, string number, string phoneNumber, string email, bool? isEnabled, int roleId)
        {
            Id = id;
            Name = name;
            _gender = gender;
            _number = number;
            _phoneNumber = phoneNumber;
            _email = email;
            _isEnabled = isEnabled;
            _roleId = roleId;
        }

        public int Id { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }

        public Gender Gender
        {
            get => _gender;
            set
            {
                if (_gender != value)
                {
                    using (var db = new Arthur.App.AppContext())
                    {
                        var user = db.Users.FirstOrDefault(o => o.Id == this.Id);
                        if (user != null)
                        {
                            user.Gender = value;
                            db.SaveChanges();
                        }
                    }
                    if (this.Id > 0)
                    {
                        Arthur.Business.Logging.AddOplog(string.Format("用户[{2}]. 性别: [{0}] 修改为 [{1}]", _gender, value, this.Name), Arthur.App.Model.OpType.编辑);
                    }
                    _gender = value;
                }
            }
        }

        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string ProfilePicture { get; set; }

        /// <summary>
        /// 用户登录密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public string Number
        {
            get => _number;
            set
            {
                if (_number != value)
                {
                    using (var db = new Arthur.App.AppContext())
                    {
                        var user = db.Users.FirstOrDefault(o => o.Id == this.Id);
                        if (user != null)
                        {
                            user.Number = value;
                            db.SaveChanges();
                        }
                    }

                    if (_number != null && this.Id > 0)
                    {
                        Arthur.Business.Logging.AddOplog(string.Format("用户[{2}]. 工号: [{0}] 修改为 [{1}]", _number, value, this.Name), Arthur.App.Model.OpType.编辑);
                    }
                    _number = value;
                }
            }
        }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if(_phoneNumber != value)
                {
                    using (var db = new Arthur.App.AppContext())
                    {
                        var user = db.Users.FirstOrDefault(o => o.Id == this.Id);
                        if (user != null)
                        {
                            user.PhoneNumber = value;
                            db.SaveChanges();
                        }
                    }

                    if (_phoneNumber != null && this.Id > 0)
                    {
                        Arthur.Business.Logging.AddOplog(string.Format("用户[{2}]. 手机号: [{0}] 修改为 [{1}]", _phoneNumber, value, this.Name), Arthur.App.Model.OpType.编辑);
                    }
                    _phoneNumber = value;
                }
            }
        }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    using (var db = new Arthur.App.AppContext())
                    {
                        var user = db.Users.FirstOrDefault(o => o.Id == this.Id);
                        if (user != null)
                        {
                            user.Email = value;
                            db.SaveChanges();
                        }
                    }

                    if (_email != null && this.Id > 0)
                    {
                        Arthur.Business.Logging.AddOplog(string.Format("用户[{2}]. 邮箱: [{0}] 修改为 [{1}]", _email, value, this.Name), Arthur.App.Model.OpType.编辑);
                    }
                    _email = value;
                }
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
            get
            {
                if (_isEnabled == null)
                {
                    _isEnabled = false;
                }
                return _isEnabled.Value;
            }

            set
            {
                if (_isEnabled != value)
                {
                    using (var db = new Arthur.App.AppContext())
                    {
                        var user = db.Users.FirstOrDefault(o => o.Id == this.Id);
                        if (user != null)
                        {
                            user.IsEnabled = value;
                            db.SaveChanges();
                        }  
                    }

                    if (_isEnabled != null && this.Id > 0)
                    {
                        Arthur.Business.Logging.AddOplog(string.Format("用户[{2}]. 是否启用: [{0}] 修改为 [{1}]", _isEnabled, value, this.Name), Arthur.App.Model.OpType.编辑);
                    }
                    _isEnabled = value;
                }
            }
        }

        // <summary>
        // 所属类别
        // </summary>
        public RoleViewModel Role
        {
            get
            {
                if (_role == null)
                {
                    using (var db = new Arthur.App.AppContext())
                    {
                        var role = db.Roles.FirstOrDefault(o => o.Id == this._roleId);
                        if (role != null)
                        {
                            _role = ContextToViewModel.Convert(role);
                        }
                    }
                }
                return _role;
            }
            set
            {
                if(_roleId != value.Id)
                {

                    using (var db = new Arthur.App.AppContext())
                    {
                        var user = db.Users.FirstOrDefault(o => o.Id == this.Id);
                        if (user != null)
                        {
                            user.Role = db.Roles.FirstOrDefault(o => o.Id == value.Id);
                            db.SaveChanges();
                        }                   
                    }

                    if (_role != null && this.Id > 0)
                    {
                        Arthur.Business.Logging.AddOplog(string.Format("用户[{2}]. 角色: [{0}] 修改为 [{1}]", _role.Name, value.Name, this.Name), Arthur.App.Model.OpType.编辑);
                    }

                    _roleId = value.Id;
                    _role = value;
                }
            }
        }

    }
}
