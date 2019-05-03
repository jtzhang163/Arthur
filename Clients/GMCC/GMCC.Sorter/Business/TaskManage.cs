using Arthur;
using GMCC.Sorter.Data;
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
            return type == TaskType.上料 && Current.Option.IsChargeGetReady
                || type == TaskType.下料 && Current.Option.IsDischargePutReady;
        }

        public static bool TaskIsFinished(TaskType type)
        {
            return type == TaskType.上料 && Current.Option.IsFeedingFinished
                || type == TaskType.下料 && Current.Option.IsBlankingFinished;
        }

        public static List<StorageViewModel> CanGetOrPutStorages(TaskType type)
        {
            var storages = new List<StorageViewModel>();

            // 满足条件的
            for (var i = 0; i < Common.STOR_COL_COUNT; i++)
            {
                for (var j = 0; j < Common.STOR_FLOOR_COUNT; j++)
                {
                    var storage = Current.Storages.First(o => o.Column == i + 1 && o.Floor == 5 - j);

                    if (type == TaskType.上料 && storage.ProcTrayId < 1)
                    {
                        storages.Add(storage);
                        continue;
                    }

                    else if (type == TaskType.下料 && storage.ProcTrayId > 0)
                    {
                        if (storage.Status == StorageStatus.静置完成)
                        {
                            storages.Add(storage);
                        }
                        continue;
                    }
                }
            }
            /// 
            /// 排序
            /// 
            return storages;
        }


        public static Result AddTaskLog()
        {
            try
            {
                Context.TaskLogs.Add(new TaskLog()
                {
                    ProcTrayId = Current.Task.ProcTrayId,
                    EndTime = DateTime.Now,
                    StartTime = Current.Task.StartTime,
                    StorageId = Current.Task.StorageId,
                    TaskType = Current.Task.Type,
                    UserId = Current.User.Id,
                    Time = DateTime.Now
                });
                Context.AppContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return new Result(ex);
            }
            return Result.OK;
        }
    }
}
