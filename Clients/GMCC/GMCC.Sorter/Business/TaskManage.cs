using GMCC.Sorter.Model;
using GMCC.Sorter.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Business
{
    public class TaskManage
    {
        public static bool BindOrUnbindMachIsReady(TaskType type)
        {
            return type == TaskType.上料 && Current.MainMachine.BindProcTrayId > 0
                || type == TaskType.下料 && Current.MainMachine.UnbindProcTrayId < 1;
        }

        public static bool TaskIsFinished(TaskType type)
        {
            return type == TaskType.上料 && Current.MainMachine.IsFeedingFinished
                || type == TaskType.下料 && Current.MainMachine.IsBlankingFinished;
        }

        public static List<StorageViewModel> CanGetOrPutStorages(TaskType type)
        {
            /// 满足条件的
            /// 
            /// 排序
            /// 
            return new List<StorageViewModel>();
        }

    }
}
