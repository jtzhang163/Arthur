using GMCC.Sorter.Data;
using GMCC.Sorter.Model;
using GMCC.Sorter.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Extensions
{
    public static class ViewModelExtension
    {
        public static ProcTray GetBindProcTray(this MainMachineViewModel mainMachine)
        {
            return Context.ProcTrays.SingleOrDefault(o => o.Id == mainMachine.BindProcTrayId) ?? new ProcTray();
        }
    }
}
