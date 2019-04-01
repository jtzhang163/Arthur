using GMCC.Sorter.Model;
using GMCC.Sorter.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Utils
{
    public static class ContextToViewModel
    {
        public static BatteryViewModel Convert(Model.Battery battery)
        {
            return new BatteryViewModel(battery.Id, battery.Code, battery.Pos, battery.ScanTime, GetObject.GetById<ProcTray>(battery.ProcTrayId).StorageId, battery.ProcTrayId, GetObject.GetById<ProcTray>(battery.ProcTrayId).StartStillTime, GetObject.GetById<ProcTray>(battery.ProcTrayId).StillTimeSpan);
        }

        public static List<BatteryViewModel> Convert(List<Model.Battery> batteries)
        {
            return batteries.ConvertAll<BatteryViewModel>(o => new BatteryViewModel(o.Id, o.Code, o.Pos, o.ScanTime, GetObject.GetById<ProcTray>(o.ProcTrayId).StorageId, o.ProcTrayId, GetObject.GetById<ProcTray>(o.ProcTrayId).StartStillTime, GetObject.GetById<ProcTray>(o.ProcTrayId).StillTimeSpan));
        }

        public static ProcTrayViewModel Convert(Model.ProcTray procTray)
        {
            return new ProcTrayViewModel(procTray.Id, procTray.Code, procTray.ScanTime, procTray.StorageId, procTray.StartStillTime, procTray.StillTimeSpan);
        }

        public static List<ProcTrayViewModel> Convert(List<Model.ProcTray> procTrays)
        {
            return procTrays.ConvertAll<ProcTrayViewModel>(o => new ProcTrayViewModel(o.Id, o.Code, o.ScanTime, o.StorageId, o.StartStillTime, o.StillTimeSpan));
        }
    }
}
