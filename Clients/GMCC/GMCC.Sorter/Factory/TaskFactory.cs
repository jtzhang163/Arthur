using GMCC.Sorter.Model;
using GMCC.Sorter.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Factory
{
    public static class TaskFactory
    {
        /// <summary>
        /// 任务类型（已排序）
        /// </summary>
        public static List<TaskType> TaskTypes
        {
            get
            {
                var types = new List<TaskType>();
                if (Current.Task.PreType == TaskType.上料)
                {
                    types.AddRange(new TaskType[] { TaskType.下料, TaskType.上料 });
                }
                else
                {
                    types.AddRange(new TaskType[] { TaskType.上料, TaskType.下料 });
                }
                return types;
            }
        }

        public static List<StorageViewModel> CanGetOrPutStorages(TaskType type)
        {
            var storages = new List<StorageViewModel>();

            // 满足条件的
            for (var i = 0; i < Common.STOR_COL_COUNT; i++)
            {
                for (var j = 0; j < Common.STOR_FLOOR_COUNT; j++)
                {
                    var storage1 = Current.Storages.First(o => o.Column == i + 1 && o.Floor == Common.STOR_FLOOR_COUNT - j);
                    var storage2 = Current.Storages.First(o => o.Column == i + 1 && o.Floor == j + 1);

                    if (type == TaskType.上料 && storage1.ProcTrayId < 1)
                    {
                        storages.Add(storage1);
                        break;
                    }
                    else if (type == TaskType.下料 && storage2.ProcTrayId > 0)
                    {
                        if (storage2.Status == StorageStatus.静置完成)
                        {
                            storages.Add(storage2);
                        }
                        break;
                    }
                }
            }
            /// 
            /// 排序
            /// 
            return storages.Where(o => o.IsEnabled).ToList();
        }

    }
}
