using GMCC.Sorter.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter
{
    /// <summary>
    /// 通用配置，一些杂项配置放置在此处
    /// </summary>
    public class AppOption : Arthur.App.AppOption
    {

        public string Name = "配置";

        private string shareConnString;
        public string ShareConnString
        {
            get
            {
                if(shareConnString == null)
                {
                    shareConnString = ConfigurationManager.ConnectionStrings["ShareDatabase"].ToString();
                }
                return shareConnString;
            }
        }



        private int taskExecInterval = -1;

        /// <summary>
        /// 搬运任务执行定时器间隔(ms)
        /// </summary>
        public int TaskExecInterval
        {
            get
            {
                if (taskExecInterval < 0)
                {
                    taskExecInterval = Arthur.Utility._Convert.StrToInt(Arthur.Business.Application.GetOption("TaskExecInterval"), -1);
                    if (taskExecInterval < 0)
                    {
                        taskExecInterval = 3000;
                        Arthur.Business.Application.SetOption("TaskExecInterval", taskExecInterval.ToString(), "搬运任务执行定时器间隔(ms)");
                    }
                }
                return taskExecInterval;
            }
            set
            {
                if (taskExecInterval != value)
                {
                    Arthur.Business.Application.SetOption("TaskExecInterval", value.ToString());
                    Arthur.Business.Logging.AddOplog(string.Format("配置. 搬运任务执行定时器间隔(ms): [{0}] 修改为 [{1}]", taskExecInterval, value), Arthur.App.Model.OpType.编辑);
                    this.SetProperty(ref taskExecInterval, value);
                }
            }
        }




        private int getShareDataExecInterval = -1;

        /// <summary>
        /// 获取共享数据执行定时器间隔(ms)
        /// </summary>
        public int GetShareDataExecInterval
        {
            get
            {
                if (getShareDataExecInterval < 0)
                {
                    getShareDataExecInterval = Arthur.Utility._Convert.StrToInt(Arthur.Business.Application.GetOption("GetShareDataExecInterval"), -1);
                    if (getShareDataExecInterval < 0)
                    {
                        getShareDataExecInterval = 3000;
                        Arthur.Business.Application.SetOption("GetShareDataExecInterval", getShareDataExecInterval.ToString(), "获取共享数据执行定时器间隔(ms)");
                    }
                }
                return getShareDataExecInterval;
            }
            set
            {
                if (getShareDataExecInterval != value)
                {
                    Arthur.Business.Application.SetOption("GetShareDataExecInterval", value.ToString());
                    Arthur.Business.Logging.AddOplog(string.Format("配置. 获取共享数据执行定时器间隔(ms): [{0}] 修改为 [{1}]", getShareDataExecInterval, value), Arthur.App.Model.OpType.编辑);
                    this.SetProperty(ref getShareDataExecInterval, value);
                }
            }
        }


        private int stillTimeSpan = -2;
        /// <summary>
        /// 静置时长(min)
        /// </summary>
        public int StillTimeSpan
        {
            get
            {
                if (stillTimeSpan == -2)
                {
                    stillTimeSpan = Arthur.Utility._Convert.StrToInt(Arthur.Business.Application.GetOption("StillTimeSpan"), -1);
                    if (stillTimeSpan == -1)
                    {
                        stillTimeSpan = 120;
                        Arthur.Business.Application.SetOption("StillTimeSpan", stillTimeSpan.ToString(), "静置时长(min)");
                    }
                }
                return stillTimeSpan;
            }
            set
            {
                if (stillTimeSpan != value)
                {
                    Arthur.Business.Application.SetOption("StillTimeSpan", value.ToString());
                    Arthur.Business.Logging.AddOplog(string.Format("设置. {0} 静置时间: [{1}] 修改为 [{2}]", Name, stillTimeSpan, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref stillTimeSpan, value);

                    //同步设置所有料仓静置时间
                    Current.Storages.ForEach(o => o.StillTimeSpan = value);
                }
            }
        }

        private int unbindProcTrayId = -2;
        /// <summary>
        /// 解盘位流程托盘Id
        /// </summary>
        public int UnbindProcTrayId
        {
            get
            {
                if (unbindProcTrayId == -2)
                {
                    unbindProcTrayId = Arthur.Utility._Convert.StrToInt(Arthur.Business.Application.GetOption("UnbindProcTrayId"), -1);
                    if (unbindProcTrayId == -1)
                    {
                        unbindProcTrayId = 0;
                        Arthur.Business.Application.SetOption("UnbindProcTrayId", unbindProcTrayId.ToString(), "解盘位流程托盘Id");
                    }
                }
                return unbindProcTrayId;
            }
            set
            {
                if (unbindProcTrayId != value)
                {
                    Arthur.Business.Application.SetOption("UnbindProcTrayId", value.ToString());
                    SetProperty(ref unbindProcTrayId, value);
                }
            }
        }


        private int jawProcTrayId = -2;
        /// <summary>
        /// 横移流程托盘Id
        /// </summary>
        public int JawProcTrayId
        {
            get
            {
                if (jawProcTrayId == -2)
                {
                    jawProcTrayId = Arthur.Utility._Convert.StrToInt(Arthur.Business.Application.GetOption("JawProcTrayId"), -1);
                    if (jawProcTrayId == -1)
                    {
                        jawProcTrayId = 0;
                        Arthur.Business.Application.SetOption("JawProcTrayId", jawProcTrayId.ToString(), "横移流程托盘Id");
                    }
                }
                return jawProcTrayId;
            }
            set
            {
                if (jawProcTrayId != value)
                {
                    Arthur.Business.Application.SetOption("JawProcTrayId", value.ToString());
                    SetProperty(ref jawProcTrayId, value);
                }
            }
        }


        private int bindBatteriesCount = -2;
        /// <summary>
        /// 当前绑盘位托盘里面电池个数
        /// </summary>
        public int BindBatteriesCount
        {
            get
            {
                if (bindBatteriesCount == -2)
                {
                    bindBatteriesCount = Current.Option.GetBindProcTray().GetBatteries().Count;
                }
                return bindBatteriesCount;
            }
            set
            {
                if (bindBatteriesCount != value)
                {
                    SetProperty(ref bindBatteriesCount, value);
                }
            }
        }



        private int bindProcTrayId = -2;
        /// <summary>
        /// 绑盘位流程托盘Id
        /// </summary>
        public int BindProcTrayId
        {
            get
            {
                if (bindProcTrayId == -2)
                {
                    bindProcTrayId = Arthur.Utility._Convert.StrToInt(Arthur.Business.Application.GetOption("BindProcTrayId"), -1);
                    if (bindProcTrayId == -1)
                    {
                        bindProcTrayId = 0;
                        Arthur.Business.Application.SetOption("BindProcTrayId", bindProcTrayId.ToString(), "绑盘位流程托盘Id");
                    }
                }
                return bindProcTrayId;
            }
            set
            {
                if (bindProcTrayId != value)
                {
                    Arthur.Business.Application.SetOption("BindProcTrayId", value.ToString());
                    SetProperty(ref bindProcTrayId, value);
                }
            }
        }


        private int chargeProcTrayId = -2;
        /// <summary>
        /// 充电位流程托盘Id
        /// </summary>
        public int ChargeProcTrayId
        {
            get
            {
                if (chargeProcTrayId == -2)
                {
                    chargeProcTrayId = Arthur.Utility._Convert.StrToInt(Arthur.Business.Application.GetOption("ChargeProcTrayId"), -1);
                    if (chargeProcTrayId == -1)
                    {
                        chargeProcTrayId = 0;
                        Arthur.Business.Application.SetOption("ChargeProcTrayId", bindProcTrayId.ToString(), "充电位流程托盘Id");
                    }
                }
                return chargeProcTrayId;
            }
            set
            {
                if (chargeProcTrayId != value)
                {
                    Arthur.Business.Application.SetOption("ChargeProcTrayId", value.ToString());
                    SetProperty(ref chargeProcTrayId, value);
                }
            }
        }


        private int dischargeProcTrayId = -2;
        /// <summary>
        /// 放电位流程托盘Id
        /// </summary>
        public int DischargeProcTrayId
        {
            get
            {
                if (dischargeProcTrayId == -2)
                {
                    dischargeProcTrayId = Arthur.Utility._Convert.StrToInt(Arthur.Business.Application.GetOption("DischargeProcTrayId"), -1);
                    if (dischargeProcTrayId == -1)
                    {
                        dischargeProcTrayId = 0;
                        Arthur.Business.Application.SetOption("DischargeProcTrayId", dischargeProcTrayId.ToString(), "放电位流程托盘Id");
                    }
                }
                return dischargeProcTrayId;
            }
            set
            {
                if (dischargeProcTrayId != value)
                {
                    Arthur.Business.Application.SetOption("DischargeProcTrayId", value.ToString());
                    SetProperty(ref dischargeProcTrayId, value);
                }
            }
        }


        public JawMoveInfo JawMoveInfo = new JawMoveInfo();

        private int jawPos = -2;
        /// <summary>
        /// 横移位置
        /// </summary>
        public int JawPos
        {
            get
            {
                if (jawPos == -2)
                {
                    jawPos = Arthur.Utility._Convert.StrToInt(Arthur.Business.Application.GetOption("JawPos"), -1);
                    if (jawPos == -1)
                    {
                        jawPos = 0;
                        Arthur.Business.Application.SetOption("JawPos", jawPos.ToString(), "横移位置");
                    }
                }
                return jawPos;
            }
            set
            {
                if (value > jawPos)
                {
                    this.JawStatus = "» 移动 »";
                }
                else if (value < jawPos)
                {
                    this.JawStatus = "« 移动 «";
                }
                else
                {
                    this.JawStatus = "闲置";
                }

                if (jawPos != value)
                {
                    Arthur.Business.Application.SetOption("JawPos", value.ToString());
                    SetProperty(ref jawPos, value);
                }
            }
        }

        private string jawStatus = "闲置";
        /// <summary>
        /// 横移状态
        /// </summary>
        public string JawStatus
        {
            get
            {
                return jawStatus;
            }
            set
            {
                if (jawStatus != value)
                {
                    SetProperty(ref jawStatus, value);
                }
            }
        }

        private bool isBatteryScanReady;
        /// <summary>
        /// 电池扫码准备就绪
        /// </summary>
        public bool IsBatteryScanReady
        {
            get => isBatteryScanReady;
            set
            {
                if (!isBatteryScanReady && value)
                {
                    IsAlreadyBatteryScan = false;
                }
                SetProperty(ref isBatteryScanReady, value);
            }
        }

        public bool IsAlreadyBatteryScan { get; set; }

        private bool isBindTrayScanReady;
        /// <summary>
        /// 绑盘托盘扫码准备就绪
        /// </summary>
        public bool IsBindTrayScanReady
        {
            get => isBindTrayScanReady;
            set
            {
                if (!isBindTrayScanReady && value)
                {
                    IsAlreadyBindTrayScan = false;
                }
                SetProperty(ref isBindTrayScanReady, value);
            }
        }

        public bool IsAlreadyBindTrayScan { get; set; }

        private bool isUnbindTrayScanReady;
        /// <summary>
        /// 解盘托盘扫码准备就绪
        /// </summary>
        public bool IsUnbindTrayScanReady
        {
            get => isUnbindTrayScanReady;
            set
            {
                if (!isUnbindTrayScanReady && value)
                {
                    isAlreadyUnbindTrayScan = false;
                }
                SetProperty(ref isUnbindTrayScanReady, value);
            }

        }




        private bool isChargeGetReady;
        /// <summary>
        /// 充电模组可取托盘
        /// </summary>
        public bool IsChargeGetReady
        {
            get => isChargeGetReady;
            set
            {
                SetProperty(ref isChargeGetReady, value);
            }
        }


        private bool isDischargePutReady;
        /// <summary>
        /// 放电模组可放托盘
        /// </summary>
        public bool IsDischargePutReady
        {
            get => isDischargePutReady;
            set
            {
                SetProperty(ref isDischargePutReady, value);
            }
        }


        private bool isHasChargeTray;
        /// <summary>
        /// 充电位有托盘
        /// </summary>
        public bool IsHasChargeTray
        {
            get => isHasChargeTray;
            set
            {
                if (!isHasChargeTray && value && Current.Option.BindBatteriesCount == Common.TRAY_BATTERY_COUNT)
                {
                    Current.Option.ChargeProcTrayId = Current.Option.BindProcTrayId;
                    Current.Option.BindProcTrayId = 0;
                }
                SetProperty(ref isHasChargeTray, value);
            }
        }

        private bool isHasDisChargeTray;
        /// <summary>
        /// 放电位有托盘
        /// </summary>
        public bool IsHasDisChargeTray
        {
            get => isHasDisChargeTray;
            set
            {
                SetProperty(ref isHasDisChargeTray, value);
            }
        }


        public bool isAlreadyUnbindTrayScan { get; set; }

        /// <summary>
        /// 横移上料完成
        /// </summary>
        public bool IsFeedingFinished { get; set; }

        /// <summary>
        /// 横移下料完成
        /// </summary>
        public bool IsBlankingFinished { get; set; }
    }
}
