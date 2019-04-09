using Arthur.App.Comm;
using Arthur.ViewModel;
using GMCC.Sorter.Data;
using GMCC.Sorter.Utils;
using GMCC.Sorter.ViewModel;
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

        private static List<StorageViewModel> storages = null;
        public static List<StorageViewModel> Storages
        {
            get
            {
                if (storages == null)
                {
                    storages = new List<StorageViewModel>();

                    Context.Storages.ToList().ForEach(o =>
                    {
                        storages.Add(new StorageViewModel(o.Id, o.Column, o.Floor, o.Name, o.Company, o.StillTimeSpan, o.ProcTrayId));
                    });
                }
                return storages;
            }
        }

        private static MainMachineViewModel mainMachine = null;
        public static MainMachineViewModel MainMachine
        {
            get
            {
                if (mainMachine == null)
                {
                    var plc = Context.PLCs.FirstOrDefault();
                    var commor = new Commor(plc);
                    mainMachine = new MainMachineViewModel(commor);
                }
                return mainMachine;
            }
        }

        private static BatteryScanerViewModel batteryScaner = null;
        public static BatteryScanerViewModel BatteryScaner
        {
            get
            {
                if (batteryScaner == null)
                {
                    var _batteryScaner = Context.BatteryScaners.FirstOrDefault();
                    var commor = new Commor(_batteryScaner);
                    batteryScaner = new BatteryScanerViewModel(commor);
                }
                return batteryScaner;
            }
        }

        public static List<TrayScanerViewModel> TrayScaners
        {
            get
            {
                return new List<TrayScanerViewModel>()
                {
                    bindTrayScaner,
                    UnbindTrayScaner
                };
            }
        }

        private static TrayScanerViewModel bindTrayScaner = null;
        /// <summary>
        /// 绑盘托盘扫码枪
        /// </summary>
        public static TrayScanerViewModel BindTrayScaner
        {
            get
            {
                if (bindTrayScaner == null)
                {
                    var _trayScaner = Context.TrayScaners.ToList()[0];
                    var commor = new Commor(_trayScaner);
                    bindTrayScaner = new TrayScanerViewModel(commor);
                }
                return bindTrayScaner;
            }
        }

        private static TrayScanerViewModel unbindTrayScaner = null;
        /// <summary>
        /// 解盘托盘扫码枪
        /// </summary>
        public static TrayScanerViewModel UnbindTrayScaner
        {
            get
            {
                if (unbindTrayScaner == null)
                {
                    var _trayScaner = Context.TrayScaners.ToList()[1];
                    var commor = new Commor(_trayScaner);
                    unbindTrayScaner = new TrayScanerViewModel(commor);
                }
                return unbindTrayScaner;
            }
        }


        private static MesViewModel mes = null;
        public static MesViewModel Mes
        {
            get
            {
                if (mes == null)
                {
                    var _mes = Context.MESs.FirstOrDefault();
                    var commor = new Commor(_mes);
                    mes = new MesViewModel(commor);
                }
                return mes;
            }
        }

        private static CurrentTaskViewModel task = null;
        public static CurrentTaskViewModel Task
        {
            get
            {
                if (task == null)
                {
                    task = new CurrentTaskViewModel(Context.CurrentTask);
                }
                return task;
            }
        }

        public static List<ShareDataViewModel> ShareDatas
        {
            get
            {
                using (var db = new ShareContext())
                {
                    var data = db.Database.SqlQuery<ShareData>("select * from t_data").ToList();
                    return ContextToViewModel.Convert(data);
                }
            }
        } 
    }
}
