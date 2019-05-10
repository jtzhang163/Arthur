using Arthur.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.App.Utils
{
    public static class ContextToViewModel
    {
        public static UserViewModel Convert(Model.User user)
        {
            return new UserViewModel(user.Id, user.Name, user.Gender, user.Number, user.PhoneNumber, user.Email, user.IsEnabled, user.RoleId);
        }

        public static List<UserViewModel> Convert(List<Model.User> users)
        {
            return users.ConvertAll<UserViewModel>(o => new UserViewModel(o.Id, o.Name, o.Gender, o.Number, o.PhoneNumber, o.Email, o.IsEnabled, o.RoleId));
        }

        public static RoleViewModel Convert(Model.Role role)
        {
            return new RoleViewModel(role.Id, role.Level, role.Name);
        }

        public static List<RoleViewModel> Convert(List<Model.Role> roles)
        {
            return roles.ConvertAll<RoleViewModel>(o => new RoleViewModel(o.Id, o.Level, o.Name));
        }
    }
}
