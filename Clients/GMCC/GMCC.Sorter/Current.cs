using Arthur.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter
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

        public static GMCC.Sorter.ViewModel.AppViewModel App = new GMCC.Sorter.ViewModel.AppViewModel();
    }
}
