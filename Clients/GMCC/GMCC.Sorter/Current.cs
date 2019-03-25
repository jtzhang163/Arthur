using Arthur.App.Comm;
using Arthur.ViewModel;
using GMCC.Sorter.Data;
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
                        storages.Add(new StorageViewModel()
                        {
                            Id = o.Id,
                            Column = o.Column,
                            Floor = o.Floor,
                            Name = o.Name,
                            Company = o.Company
                        });
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
                if(mainMachine == null)
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
                    FormerTrayScaner,
                    LatterTrayScaner
                };
            }
        }

        private static TrayScanerViewModel formerTrayScaner = null;
        public static TrayScanerViewModel FormerTrayScaner
        {
            get
            {
                if (formerTrayScaner == null)
                {
                    var _trayScaner = Context.TrayScaners.ToList()[0];
                    var commor = new Commor(_trayScaner);
                    formerTrayScaner = new TrayScanerViewModel(commor);
                }
                return formerTrayScaner;
            }
        }

        private static TrayScanerViewModel latterTrayScaner = null;
        public static TrayScanerViewModel LatterTrayScaner
        {
            get
            {
                if (latterTrayScaner == null)
                {
                    var _trayScaner = Context.TrayScaners.ToList()[1];
                    var commor = new Commor(_trayScaner);
                    latterTrayScaner = new TrayScanerViewModel(commor);
                }
                return latterTrayScaner;
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
    }
}
