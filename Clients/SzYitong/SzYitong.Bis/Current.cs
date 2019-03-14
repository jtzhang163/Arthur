using Arthur.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzYitong.Bis
{
    public static class Current
    {
        public static Arthur.App.Model.User User
        {
            get
            {
                return Arthur.App.Current.User;
            }
        }

        public static AppOption Option = new AppOption();

        public static SzYitong.Bis.ViewModel.AppViewModel App = new SzYitong.Bis.ViewModel.AppViewModel();
    }
}
