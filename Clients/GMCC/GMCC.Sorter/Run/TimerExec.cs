using Arthur.Core;
using Arthur.App.Model;
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
using GMCC.Sorter.Other;

namespace GMCC.Sorter.Run
{
    public static class TimerExec
    {
        public static bool IsRunning { get; set; }

        public static void TaskExec(object obj)
        {
            if (!IsRunning) return;

            try
            {
                if (Current.Task.Status == Model.TaskStatus.完成)
                {
                    if (Current.App.TaskMode == ViewModel.TaskMode.自动任务)
                    {
                        foreach (var type in Factory.TaskFactory.TaskTypes)
                        {
                            if (type == TaskType.上料 && Current.Option.Tray13_Id < 1)
                            {
                                continue;
                            }
                            else if (type == TaskType.下料 && Current.Option.Tray21_Id > 0)
                            {
                                continue;
                            }

                            if (Current.Option.IsTaskReady)
                            {
                                var storages = Factory.TaskFactory.CanGetOrPutStorages(type);
                                if (storages.Count > 0)
                                {
                                    StorageViewModel storage = null;

                                    if (Current.Option.TaskPriorityType == Other.TaskPriorityType.层优先)
                                    {
                                        if (type == TaskType.上料)
                                        {
                                            storage = storages.OrderByDescending(o => o.Floor).First();
                                        }
                                        else
                                        {
                                            storage = storages.OrderBy(o => o.Floor).First();
                                        }
                                    }
                                    else
                                    {
                                        if (type == TaskType.上料)
                                        {
                                            storage = storages.OrderBy(o => o.Column).First();
                                        }
                                        else
                                        {
                                            storage = storages.OrderByDescending(o => o.Column).First();
                                        }
                                    }

                                    Current.Task.StorageId = storage.Id;
                                    Current.Task.Type = type;
                                    Current.Task.StartTime = DateTime.Now;
                                    Current.Task.ProcTrayId = type == Model.TaskType.上料 ? Current.Option.Tray13_Id : storage.ProcTrayId;
                                    Current.Task.Status = Model.TaskStatus.就绪;

                                    LogHelper.WriteInfo(string.Format("=== 生成自动任务 类型：{0}，料仓：{1}，流程托盘ID：{2}，托盘条码：{3} ===",
                                        Current.Task.Type, storage.Name, Current.Task.ProcTrayId, GetObject.GetById<ProcTray>(Current.Task.ProcTrayId).Code));

                                    break;
                                }
                            }
                        }
                    }
                }
                else if (Current.Task.Status == Model.TaskStatus.就绪)
                {
                    var storage = GetObject.GetById<StorageViewModel>(Current.Task.StorageId);
                    var toMoveInfo = JawMoveInfo.Create(Current.Task.Type, storage);

                    //若指令已经发给PLC
                    if (Current.Option.JawMoveInfo.Equals(toMoveInfo))
                    {
                        Current.Option.JawProcTrayId = Current.Task.ProcTrayId;
                        Current.Task.Status = Model.TaskStatus.准备搬;
                        return;
                    }

                    Current.MainMachine.SendCommand(toMoveInfo);

                }
                else if (Current.Task.Status == Model.TaskStatus.准备搬)
                {
                    //若指令已经发给PLC
                    if (Current.Option.IsJawHasTray)
                    {
                        Current.Task.Status = Model.TaskStatus.搬运中;
                        return;
                    }
                }
                else if (Current.Task.Status == Model.TaskStatus.搬运中)
                {
                    if (Current.Option.IsTaskFinished)
                    {
                        var storage = GetObject.GetById<StorageViewModel>(Current.Task.StorageId);
                        if (Current.Task.Type == Model.TaskType.上料)
                        {
                            storage.ProcTrayId = Current.Task.ProcTrayId;
                            storage.ProcTray.StartStillTime = DateTime.Now;
                        }
                        else
                        {
                            Current.Option.Tray21_Id = Current.Task.ProcTrayId;
                            if(storage.ProcTrayId > 0)
                            {
                                storage.ProcTray.StillTimeSpan = Convert.ToInt32((DateTime.Now - storage.ProcTray.StartStillTime).TotalMinutes);
                                storage.ProcTrayId = 0;
                            }
                        }
                        Current.Option.JawProcTrayId = 0;
                        Current.Task.Status = Model.TaskStatus.回位中;
                    }
                }
                else if (Current.Task.Status == Model.TaskStatus.回位中)
                {
                    if (Current.Option.IsTaskReady)
                    {
                        Current.Task.PreType = Current.Task.Type;
                        Current.Task.Status = Model.TaskStatus.完成;
                        new TaskManage().AddTaskLog();
                    }
                }
            }
            catch (Exception ex)
            {
                Running.StopRunAndShowMsg("执行任务出现异常：" + ex.Message);
                LogHelper.WriteError(ex);
            }
        }

        public static void GetShareDataExec(object obj)
        {
            if (!IsRunning) return;

            if (Current.ShareDatas.Count > 0)
            {
                if (Current.Option.Tray12_Id > 0)
                {
                    var chargeData = Current.ShareDatas.First(o => o.Key == "chargeCodes");
                    var bindCode = JsonHelper.DeserializeJsonToObject<BindCode>(chargeData.Value);
                    var procTray = GetObject.GetById<ProcTray>(Current.Option.Tray12_Id);

                    if (procTray.Id > 0 && chargeData.Status == 2)
                    {
                        if (procTray.Id == chargeData.ProcTrayId)
                        {

                        }
                        else
                        {

                            //充电位条码绑定信息传给BTS客户端
                            var batteries = procTray.GetBatteries();
                            var codes = new List<string>();
                            for (var i = 1; i <= Common.TRAY_BATTERY_COUNT; i++)
                            {
                                var code = "";
                                var battery = batteries.FirstOrDefault(o => o.GetChargeOrder() == i);
                                if (battery != null)
                                {
                                    code = battery.Code;
                                }
                                codes.Add(code);
                            }

                            var value = new BindCode { TrayCode = procTray.Code, BatteryCodes = string.Join(",", codes.ToArray()) };
                            chargeData.Value = JsonHelper.SerializeObject(value);
                            chargeData.Status = 1;
                            chargeData.ProcTrayId = procTray.Id;
                            chargeData.UpdateTime = DateTime.Now;
                            LogHelper.WriteInfo(string.Format("--------成功发送充电位条码绑定信息给BTS【流程托盘ID：{0}，条码：{1}】---------", procTray.Id, procTray.Code));
                        }
                    }
                }

                if (Current.Option.Tray22_Id > 0)
                {
                    var dischargeData = Current.ShareDatas.First(o => o.Key == "dischargeCodes");
                    var bindCode = JsonHelper.DeserializeJsonToObject<BindCode>(dischargeData.Value);
                    var procTray = GetObject.GetById<ProcTray>(Current.Option.Tray22_Id);

                    if (procTray.Id > 0 && dischargeData.Status == 2)
                    {
                        if (procTray.Id == dischargeData.ProcTrayId)
                        {

                        }
                        else
                        {
                            //放电位条码绑定信息传给BTS客户端
                            var batteries = procTray.GetBatteries();
                            var codes = new List<string>();
                            for (var i = 1; i <= Common.TRAY_BATTERY_COUNT; i++)
                            {
                                var code = "";
                                var battery = batteries.FirstOrDefault(o => o.GetChargeOrder() == i);
                                if (battery != null)
                                {
                                    code = battery.Code;
                                }
                                codes.Add(code);
                            }

                            var value = new BindCode { TrayCode = procTray.Code, BatteryCodes = string.Join(",", codes.ToArray()) };
                            dischargeData.Value = JsonHelper.SerializeObject(value);
                            dischargeData.Status = 1;
                            dischargeData.ProcTrayId = procTray.Id;
                            dischargeData.UpdateTime = DateTime.Now;
                            LogHelper.WriteInfo(string.Format("--------成功发送放电位条码绑定信息给BTS【流程托盘ID：{0}，条码：{1}】---------", procTray.Id, procTray.Code));
                        }
                    }
                }

                if (Current.Option.Tray23_Id > 0)
                {

                    var sortingResults = Current.ShareDatas.First(o => o.Key == "sortingResults");
                    var bindResults = JsonHelper.DeserializeJsonToObject<SortingResult>(sortingResults.Value);
                    var procTray = GetObject.GetById<ProcTray>(Current.Option.Tray23_Id);

                    if (sortingResults.Status == 1)
                    {
                        if (bindResults.TrayCode == procTray.Code)
                        {
                            var results = bindResults.Results.Split(',');
                            for (int i = 0; i < results.Length; i++)
                            {
                                //i:绑盘序号
                                Current.MainMachine.Commor.Write(string.Format("D{0:D3}", 401 + i), ushort.Parse(results[OrderManage.GetChargeOrderBySortOrder(i + 1) - 1]));
                            }
                            LogHelper.WriteInfo(string.Format("--------成功发送分选结果数据给PLC【流程托盘ID：{0}，条码：{1}】---------", procTray.Id, procTray.Code));

                            var batteries = procTray.GetBatteries();
                            var batteryViewModels = ContextToViewModel.Convert(batteries);

                            for (int i = 0; i < results.Length; i++)
                            {
                                //i:通道序号
                                var result = int.Parse(results[i]);
                                if (result > 0)
                                {
                                    var battery = batteryViewModels.FirstOrDefault(o => o.Pos == OrderManage.GetBindOrderByChargeOrder(i + 1));
                                    if (battery != null)
                                    {
                                        battery.SortResult = (SortResult)result;
                                    }
                                }
                            }

                            sortingResults.Status = 2;
                            sortingResults.ProcTrayId = procTray.Id;
                            sortingResults.UpdateTime = DateTime.Now;

                        }

                    }
                }
            }

        }
    }
}
