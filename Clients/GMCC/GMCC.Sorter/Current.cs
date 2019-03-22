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

        private static StorageViewModel[,] storages = null;
        public static StorageViewModel[,] Storages
        {
            get
            {
                if (storages == null)
                {
                    storages = new ViewModel.StorageViewModel[Common.STOR_COL_COUNT, Common.STOR_FLOOR_COUNT];
                    for (var i = 0; i < Common.STOR_COL_COUNT; i++)
                    {
                        for (int j = 0; j < Common.STOR_FLOOR_COUNT; j++)
                        {
                            storages[i, j] = new ViewModel.StorageViewModel(i + 1, j + 1);
                            storages[i, j].Name = Context.Storages.FirstOrDefault(o => o.Column == i + 1 && o.Floor == j + 1).Name;
                        }
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
                if(mainMachine == null)
                {
                    var plc = Context.PLCs.FirstOrDefault();
                    var commor = new Commor(plc);
                    mainMachine = new MainMachineViewModel(commor);
                }
                return mainMachine;
            }
        }
    }
}
