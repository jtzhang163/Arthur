using GMCC.Sorter.Business;
using GMCC.Sorter.Factory;
using GMCC.Sorter.Utils;
using GMCC.Sorter.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace GMCC.Sorter.Run
{
    public class TimerExec
    {
        public static bool IsRunning { get; set; }

        public static void TaskExec(object obj)
        {
            if (Current.Task.Status == Model.TaskStatus.完成)
            {
                if (Current.App.TaskMode == ViewModel.TaskMode.自动任务)
                {
                    foreach (var type in TaskHelper.TaskTypes)
                    {
                        if (TaskManage.BindOrUnbindMachIsReady(type))
                        {
                            var storages = TaskManage.CanGetOrPutStorages(type);
                            if (storages.Count > 0)
                            {
                                var storage = storages.First();
                                Current.Task.StorageId = storage.Id;
                                Current.Task.Type = type;
                                Current.Task.StartTime = DateTime.Now;
                                Current.Task.ProcTrayId = type == Model.TaskType.上料 ? Current.MainMachine.BindProcTrayId : storage.ProcTrayId;
                                Current.Task.Status = Model.TaskStatus.就绪;
                                break;
                            }
                        }
                    }
                }
                else
                {

                }
            }
            else if (Current.Task.Status == Model.TaskStatus.就绪)
            {
                var storage = GetObject.GetById<StorageViewModel>(Current.Task.StorageId);
                var toMoveInfo = JawMoveInfo.Create(Current.Task.Type, storage);

                //若指令已经发给PLC
                if (Current.MainMachine.JawMoveInfo.Equals(toMoveInfo))
                {
                    Current.MainMachine.JawProcTrayId = Current.Task.ProcTrayId;
                    Current.Task.Status = Model.TaskStatus.执行中;
                    return;
                }

                Current.MainMachine.SendCommand(toMoveInfo);

            }
            else if (Current.Task.Status == Model.TaskStatus.执行中)
            {
                if (TaskManage.TaskIsFinished(Current.Task.Type))
                {
                    if (Current.Task.Type == Model.TaskType.上料)
                    {
                        var storage = GetObject.GetById<StorageViewModel>(Current.Task.StorageId);
                        storage.ProcTrayId = Current.Task.ProcTrayId;
                    }
                    else
                    {
                        Current.MainMachine.UnbindProcTrayId = Current.Task.ProcTrayId;
                    }

                    Current.Task.PreType = Current.Task.Type;
                    Current.Task.Status = Model.TaskStatus.完成;
                }
            }
        }
    }
}
