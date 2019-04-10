using Arthur.Utils;
using GMCC.Sorter.Business;
using GMCC.Sorter.Data;
using GMCC.Sorter.Extensions;
using GMCC.Sorter.Factory;
using GMCC.Sorter.Model;
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
            if (!IsRunning) return;

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
                                Current.Task.ProcTrayId = type == Model.TaskType.上料 ? Current.MainMachine.ChargeProcTrayId : storage.ProcTrayId;
                                Current.Task.Status = Model.TaskStatus.就绪;
                                Context.AppContext.SaveChanges();
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
                    Context.AppContext.SaveChanges();
                    return;
                }

                Current.MainMachine.SendCommand(toMoveInfo);

            }
            else if (Current.Task.Status == Model.TaskStatus.执行中)
            {
                if (TaskManage.TaskIsFinished(Current.Task.Type))
                {
                    var storage = GetObject.GetById<StorageViewModel>(Current.Task.StorageId);
                    if (Current.Task.Type == Model.TaskType.上料)
                    {
                        storage.ProcTrayId = Current.Task.ProcTrayId;
                        storage.ProcTray.StartStillTime = DateTime.Now;
                        Current.MainMachine.ChargeProcTrayId = 0;
                    }
                    else
                    {
                        storage.ProcTrayId = 0;
                        Current.MainMachine.DischargeProcTrayId = Current.Task.ProcTrayId;
                    }

                    TaskManage.AddTaskLog();

                    Current.MainMachine.JawProcTrayId = 0;
                    Current.Task.PreType = Current.Task.Type;
                    Current.Task.Status = Model.TaskStatus.完成;
                    Context.AppContext.SaveChanges();
                }
            }
        }

        public static void GetShareDataExec(object obj)
        {
            if (!IsRunning) return;

            var isChargeFinished = false;

            if (Current.ShareDatas.Count > 0)
            {
                if(Current.MainMachine.ChargeProcTrayId > 0)
                {
                    var chargeData = Current.ShareDatas.First(o => o.Key == "ChargeCodes");
                    var bindCode = JsonHelper.DeserializeJsonToObject<BindCode>(chargeData.Value);
                    var procTray = GetObject.GetById<ProcTray>(Current.MainMachine.ChargeProcTrayId);
                    if (chargeData.Status == 2)
                    {
                        if (bindCode.TrayCode == procTray.Code)
                        {
                            //充电位置可以取盘
                            isChargeFinished = true;
                        }
                        else
                        {
                            //充电位条码绑定信息传给BTS客户端
                            var codes = procTray.GetBatteries().ConvertAll<string>(o => o.Code);
                            var value = new BindCode { TrayCode = procTray.Code, BatteryCodes = string.Join(",", codes.ToArray()) };
                            chargeData.Value = JsonHelper.SerializeObject(value);
                            chargeData.Status = 1;
                        }

                    }
                }

                Current.MainMachine.IsChargeGetReady = isChargeFinished;


                if (Current.MainMachine.DischargeProcTrayId > 0)
                {
                    var dischargeData = Current.ShareDatas.First(o => o.Key == "DischargeCodes");
                    var bindCode = JsonHelper.DeserializeJsonToObject<BindCode>(dischargeData.Value);
                    var procTray = GetObject.GetById<ProcTray>(Current.MainMachine.DischargeProcTrayId);
                    if (dischargeData.Status == 2)
                    {
                        if (bindCode.TrayCode == procTray.Code)
                        {

                        }
                        else
                        {
                            //充电位条码绑定信息传给BTS客户端
                            var codes = procTray.GetBatteries().ConvertAll<string>(o => o.Code);
                            var value = new BindCode { TrayCode = procTray.Code, BatteryCodes = string.Join(",", codes.ToArray()) };
                            dischargeData.Value = JsonHelper.SerializeObject(value);
                            dischargeData.Status = 1;
                        }

                    }
                }

            }


        }
    }
}
