using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arthur.App.Data;
using Arthur.App.Model;
using Arthur.App.ViewModel;

namespace Arthur.App
{
    public static class Current
    {

        public static UserViewModel User;

        public static AppOption Option = new AppOption();

    }
}
