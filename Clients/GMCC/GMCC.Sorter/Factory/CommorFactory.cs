using GMCC.Sorter.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Factory
{
    public static class CommorFactory
    {
        public static List<CommorViewModel> GetCommors()
        {
            return new List<CommorViewModel>()
            {
                Current.MainMachine,
                Current.BatteryScaner,
                Current.BindTrayScaner,
                Current.UnbindTrayScaner,
                Current.Mes,
            };
        }
    }
}
