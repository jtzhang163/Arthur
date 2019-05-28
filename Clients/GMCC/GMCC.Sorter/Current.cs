using Arthur.App.Comm;
using Arthur.App.ViewModel;
using GMCC.Sorter.Data;
using GMCC.Sorter.Other;
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
        public static Arthur.App.ViewModel.UserViewModel User
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

                    using (var db = new Data.AppContext())
                    {
                        db.Storages.ToList().ForEach(o =>
                        {
                            storages.Add(new StorageViewModel(o.Id, o.Column, o.Floor, o.Name, o.Company, o.StillTimeSpan, o.ProcTrayId, o.IsEnabled));
                        });
                    }
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
                    using (var db = new Data.AppContext())
                    {
                        var plc = db.PLCs.FirstOrDefault();
                        var commor = new Commor(plc);
                        mainMachine = new MainMachineViewModel(commor);
                    }
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
                    using (var db = new Data.AppContext())
                    {
                        var _batteryScaner = db.BatteryScaners.FirstOrDefault();
                        var commor = new Commor(_batteryScaner);
                        batteryScaner = new BatteryScanerViewModel(commor);
                    }
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
                    using (var db = new Data.AppContext())
                    {
                        var _trayScaner = db.TrayScaners.ToList()[0];
                        var commor = new Commor(_trayScaner);
                        bindTrayScaner = new TrayScanerViewModel(commor);
                    }
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
                    using (var db = new Data.AppContext())
                    {
                        var _trayScaner = db.TrayScaners.ToList()[1];
                        var commor = new Commor(_trayScaner);
                        unbindTrayScaner = new TrayScanerViewModel(commor);
                    }
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
                    using (var db = new Data.AppContext())
                    {
                        var _mes = db.MESs.FirstOrDefault();
                        var commor = new Commor(_mes);
                        mes = new MesViewModel(commor);
                    }
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
                    using (var db = new Data.AppContext())
                    {
                        task = new CurrentTaskViewModel(db.CurrentTasks.FirstOrDefault());
                    }
                }
                return task;
            }
        }

        public static List<NewareDataViewModel> ShareDatas
        {
            get
            {
                using (var db = new NewareContext())
                {
                    var data = db.Database.SqlQuery<NewareData>("select * from t_data").ToList();
                    return ContextToViewModel.Convert(data);
                }
            }
        }

        private static List<SortPackViewModel> sortPacks;
        public static List<SortPackViewModel> SortPacks
        {
            get
            {
                if (sortPacks == null)
                {
                    sortPacks = new List<SortPackViewModel>();
                    var random = new Random();
                    for (int i = 0; i < 5; i++)
                    {
                        sortPacks.Add(new SortPackViewModel()
                        {
                            Type = string.Format("{0}档", i + 1),
                            Count = random.Next(300)
                        });
                    }
                }
                return sortPacks;
            }
        }
    }
}
