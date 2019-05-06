using Arthur.App;
using GMCC.Sorter.Extensions;
using GMCC.Sorter.Model;
using GMCC.Sorter.Utils;
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
    public class AppOption : BindableObject
    {

        public string Name = "配置";

        private string shareConnString;
        public string ShareConnString
        {
            get
            {
                if (shareConnString == null)
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


        private int bindBatteriesCount = -2;
        /// <summary>
        /// 当前绑盘位托盘里面电池个数
        /// </summary>
        public int BindBatteriesCount
        {
            get
            {
                if (bindBatteriesCount < 0)
                {
                    bindBatteriesCount = GetObject.GetById<ProcTray>(Current.Option.Tray11_Id).GetBatteries().Count;
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


        /// <summary>
        /// 上下料任务准备就绪
        /// </summary>
        public bool IsTaskReady { get; set; }

        /// <summary>
        /// 上下料任务完成
        /// </summary>
        public bool IsTaskFinished { get; set; }


        #region 横移相关

        public JawMoveInfo JawMoveInfo = new JawMoveInfo();

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
                else if (Current.Task.Status != Model.TaskStatus.完成)
                {
                    this.JawStatus = "取放中";
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

        private bool isJawHasTray;
        /// <summary>
        /// 横移有托盘
        /// </summary>
        public bool IsJawHasTray
        {
            get => isJawHasTray;
            set
            {
                SetProperty(ref isJawHasTray, value);
            }
        }

        #endregion

        #region 工位有托盘信号

        private bool? isHasTray11;
        /// <summary>
        /// 绑盘位有托盘
        /// </summary>
        public bool IsHasTray11
        {
            get => isHasTray11.HasValue ? isHasTray11.Value : false;
            set
            {
                if (value)
                {

                }
                else
                {
                    if (this.Tray11_Id > 0)
                    {
                        this.Tray11_PreId = this.Tray11_Id;
                        this.Tray11_Id = 0;
                    }
                }

                SetProperty(ref isHasTray11, value);
            }
        }

        private bool? isHasTray12;
        /// <summary>
        /// 充电位有托盘
        /// </summary>
        public bool IsHasTray12
        {
            get => isHasTray12.HasValue ? isHasTray12.Value : false;
            set
            {
                if (value)
                {
                    if (this.Tray12_Id < 1 && this.Tray11_PreId > 0)
                    {
                        this.Tray12_Id = this.Tray11_PreId;
                        this.Tray11_PreId = 0;
                    }
                }
                else
                {
                    if (this.Tray12_Id > 0)
                    {
                        this.Tray12_PreId = this.Tray12_Id;
                        this.Tray12_Id = 0;
                    }
                }

                SetProperty(ref isHasTray12, value);
            }
        }

        private bool? isHasTray13;
        /// <summary>
        /// 上料位有托盘
        /// </summary>
        public bool IsHasTray13
        {
            get => isHasTray13.HasValue ? isHasTray13.Value : false;
            set
            {
                if (value)
                {
                    if (this.Tray13_Id < 1 && this.Tray12_PreId > 0)
                    {
                        this.Tray13_Id = this.Tray12_PreId;
                        this.Tray12_PreId = 0;
                    }
                }
                else
                {
                    if (this.Tray13_Id > 0)
                    {
                        //this.Tray13_PreId = this.Tray13_Id;
                        this.Tray13_Id = 0;
                    }
                }

                SetProperty(ref isHasTray13, value);
            }
        }


        private bool? isHasTray21;
        /// <summary>
        /// 下料位有托盘
        /// </summary>
        public bool IsHasTray21
        {
            get => isHasTray21.HasValue ? isHasTray21.Value : false;
            set
            {
                if (value)
                {

                }
                else
                {
                    if (this.Tray21_Id > 0)
                    {
                        this.Tray21_PreId = this.Tray21_Id;
                        this.Tray21_Id = 0;
                    }
                }

                SetProperty(ref isHasTray21, value);
            }
        }

        private bool? isHasTray22;
        /// <summary>
        /// 放电位有托盘
        /// </summary>
        public bool IsHasTray22
        {
            get => isHasTray22.HasValue ? isHasTray22.Value : false;
            set
            {
                if (value)
                {
                    if (this.Tray22_Id < 1 && this.Tray21_PreId > 0)
                    {
                        this.Tray22_Id = this.Tray21_PreId;
                        this.Tray21_PreId = 0;
                    }
                }
                else
                {
                    if (this.Tray22_Id > 0)
                    {
                        this.Tray22_PreId = this.Tray22_Id;
                        this.Tray22_Id = 0;
                    }
                }

                SetProperty(ref isHasTray22, value);
            }
        }


        private bool? isHasTray23;
        /// <summary>
        /// 分选位有托盘
        /// </summary>
        public bool IsHasTray23
        {
            get => isHasTray23.HasValue ? isHasTray23.Value : false;
            set
            {
                if (value)
                {
                    if (this.Tray23_Id < 1 && this.Tray22_PreId > 0)
                    {
                        this.Tray23_Id = this.Tray22_PreId;
                        this.Tray22_PreId = 0;
                    }
                }
                else
                {
                    if (this.Tray23_Id > 0)
                    {
                        //this.Tray23_PreId = this.Tray23_Id;
                        this.Tray23_Id = 0;
                    }
                }

                SetProperty(ref isHasTray23, value);
            }
        }


        #endregion

        #region 工位流程托盘ID

        private int tray11_Id = -2;
        /// <summary>
        /// 绑盘位流程托盘Id
        /// </summary>
        public int Tray11_Id
        {
            get
            {
                if (tray11_Id == -2)
                {
                    tray11_Id = Arthur.Utility._Convert.StrToInt(Arthur.Business.Application.GetOption("Tray11_Id"), -1);
                    if (tray11_Id == -1)
                    {
                        tray11_Id = 0;
                        Arthur.Business.Application.SetOption("Tray11_Id", tray11_Id.ToString(), "绑盘位流程托盘Id");
                    }
                }
                return tray11_Id;
            }
            set
            {
                if (tray11_Id != value)
                {
                    Arthur.Business.Application.SetOption("Tray11_Id", value.ToString());
                    SetProperty(ref tray11_Id, value);
                }
            }
        }


        private int tray12_Id = -2;
        /// <summary>
        /// 充电位流程托盘Id
        /// </summary>
        public int Tray12_Id
        {
            get
            {
                if (tray12_Id == -2)
                {
                    tray12_Id = Arthur.Utility._Convert.StrToInt(Arthur.Business.Application.GetOption("Tray12_Id"), -1);
                    if (tray12_Id == -1)
                    {
                        tray12_Id = 0;
                        Arthur.Business.Application.SetOption("Tray12_Id", tray12_Id.ToString(), "充电位流程托盘Id");
                    }
                }
                return tray12_Id;
            }
            set
            {
                if (tray12_Id != value)
                {
                    Arthur.Business.Application.SetOption("Tray12_Id", value.ToString());
                    SetProperty(ref tray12_Id, value);
                }
            }
        }


        private int tray13_Id = -2;
        /// <summary>
        /// 上料位流程托盘Id
        /// </summary>
        public int Tray13_Id
        {
            get
            {
                if (tray13_Id == -2)
                {
                    tray13_Id = Arthur.Utility._Convert.StrToInt(Arthur.Business.Application.GetOption("Tray13_Id"), -1);
                    if (tray13_Id == -1)
                    {
                        tray13_Id = 0;
                        Arthur.Business.Application.SetOption("Tray13_Id", tray13_Id.ToString(), "上料位流程托盘Id");
                    }
                }
                return tray13_Id;
            }
            set
            {
                if (tray13_Id != value)
                {
                    Arthur.Business.Application.SetOption("Tray13_Id", value.ToString());
                    SetProperty(ref tray13_Id, value);
                }
            }
        }


        private int tray21_Id = -2;
        /// <summary>
        /// 下料位流程托盘Id
        /// </summary>
        public int Tray21_Id
        {
            get
            {
                if (tray21_Id == -2)
                {
                    tray21_Id = Arthur.Utility._Convert.StrToInt(Arthur.Business.Application.GetOption("Tray21_Id"), -1);
                    if (tray21_Id == -1)
                    {
                        tray21_Id = 0;
                        Arthur.Business.Application.SetOption("Tray21_Id", tray21_Id.ToString(), "下料位流程托盘Id");
                    }
                }
                return tray21_Id;
            }
            set
            {
                if (tray21_Id != value)
                {
                    Arthur.Business.Application.SetOption("Tray21_Id", value.ToString());
                    SetProperty(ref tray21_Id, value);
                }
            }
        }

        private int tray22_Id = -2;
        /// <summary>
        /// 放电位流程托盘Id
        /// </summary>
        public int Tray22_Id
        {
            get
            {
                if (tray22_Id == -2)
                {
                    tray22_Id = Arthur.Utility._Convert.StrToInt(Arthur.Business.Application.GetOption("Tray22_Id"), -1);
                    if (tray22_Id == -1)
                    {
                        tray22_Id = 0;
                        Arthur.Business.Application.SetOption("Tray22_Id", tray22_Id.ToString(), "放电位流程托盘Id");
                    }
                }
                return tray22_Id;
            }
            set
            {
                if (tray22_Id != value)
                {
                    Arthur.Business.Application.SetOption("Tray22_Id", value.ToString());
                    SetProperty(ref tray22_Id, value);
                }
            }
        }


        private int tray23_Id = -2;
        /// <summary>
        /// 分选位流程托盘Id
        /// </summary>
        public int Tray23_Id
        {
            get
            {
                if (tray23_Id == -2)
                {
                    tray23_Id = Arthur.Utility._Convert.StrToInt(Arthur.Business.Application.GetOption("Tray23_Id"), -1);
                    if (tray23_Id == -1)
                    {
                        tray23_Id = 0;
                        Arthur.Business.Application.SetOption("Tray23_Id", tray23_Id.ToString(), "分选位流程托盘Id");
                    }
                }
                return tray23_Id;
            }
            set
            {
                if (tray23_Id != value)
                {
                    Arthur.Business.Application.SetOption("Tray23_Id", value.ToString());
                    SetProperty(ref tray23_Id, value);
                }
            }
        }

        #endregion

        #region 工位流程托盘原ID

        private int tray11_PreId = -2;
        /// <summary>
        /// 绑盘位流程托盘原ID
        /// </summary>
        public int Tray11_PreId
        {
            get
            {
                if (tray11_PreId == -2)
                {
                    tray11_PreId = Arthur.Utility._Convert.StrToInt(Arthur.Business.Application.GetOption("Tray11_PreId"), -1);
                    if (tray11_PreId == -1)
                    {
                        tray11_PreId = 0;
                        Arthur.Business.Application.SetOption("Tray11_PreId", tray11_PreId.ToString(), "绑盘位流程托盘原ID");
                    }
                }
                return tray11_PreId;
            }
            set
            {
                if (tray11_PreId != value)
                {
                    Arthur.Business.Application.SetOption("Tray11_PreId", value.ToString());
                    SetProperty(ref tray11_PreId, value);
                }
            }
        }


        private int tray12_PreId = -2;
        /// <summary>
        /// 充电位流程托盘原ID
        /// </summary>
        public int Tray12_PreId
        {
            get
            {
                if (tray12_PreId == -2)
                {
                    tray12_PreId = Arthur.Utility._Convert.StrToInt(Arthur.Business.Application.GetOption("Tray12_PreId"), -1);
                    if (tray12_PreId == -1)
                    {
                        tray12_PreId = 0;
                        Arthur.Business.Application.SetOption("Tray12_PreId", tray12_PreId.ToString(), "充电位流程托盘原ID");
                    }
                }
                return tray12_PreId;
            }
            set
            {
                if (tray12_PreId != value)
                {
                    Arthur.Business.Application.SetOption("Tray12_PreId", value.ToString());
                    SetProperty(ref tray12_PreId, value);
                }
            }
        }


        //private int tray13_PreId = -2;
        ///// <summary>
        ///// 上料位流程托盘原ID
        ///// </summary>
        //public int Tray13_PreId
        //{
        //    get
        //    {
        //        if (tray13_PreId == -2)
        //        {
        //            tray13_PreId = Arthur.Utility._Convert.StrToInt(Arthur.Business.Application.GetOption("Tray13_PreId"), -1);
        //            if (tray13_PreId == -1)
        //            {
        //                tray13_PreId = 0;
        //                Arthur.Business.Application.SetOption("Tray13_PreId", tray13_PreId.ToString(), "上料位流程托盘原ID");
        //            }
        //        }
        //        return tray13_PreId;
        //    }
        //    set
        //    {
        //        if (tray13_PreId != value)
        //        {
        //            Arthur.Business.Application.SetOption("Tray13_PreId", value.ToString());
        //            SetProperty(ref tray13_PreId, value);
        //        }
        //    }
        //}


        private int tray21_PreId = -2;
        /// <summary>
        /// 下料位流程托盘原ID
        /// </summary>
        public int Tray21_PreId
        {
            get
            {
                if (tray21_PreId == -2)
                {
                    tray21_PreId = Arthur.Utility._Convert.StrToInt(Arthur.Business.Application.GetOption("Tray21_PreId"), -1);
                    if (tray21_PreId == -1)
                    {
                        tray21_PreId = 0;
                        Arthur.Business.Application.SetOption("Tray21_PreId", tray21_PreId.ToString(), "下料位流程托盘原ID");
                    }
                }
                return tray21_PreId;
            }
            set
            {
                if (tray21_PreId != value)
                {
                    Arthur.Business.Application.SetOption("Tray21_PreId", value.ToString());
                    SetProperty(ref tray21_PreId, value);
                }
            }
        }

        private int tray22_PreId = -2;
        /// <summary>
        /// 放电位流程托盘原ID
        /// </summary>
        public int Tray22_PreId
        {
            get
            {
                if (tray22_PreId == -2)
                {
                    tray22_PreId = Arthur.Utility._Convert.StrToInt(Arthur.Business.Application.GetOption("Tray22_PreId"), -1);
                    if (tray22_PreId == -1)
                    {
                        tray22_PreId = 0;
                        Arthur.Business.Application.SetOption("Tray22_PreId", tray22_PreId.ToString(), "放电位流程托盘原ID");
                    }
                }
                return tray22_PreId;
            }
            set
            {
                if (tray22_PreId != value)
                {
                    Arthur.Business.Application.SetOption("Tray22_PreId", value.ToString());
                    SetProperty(ref tray22_PreId, value);
                }
            }
        }


        //private int tray23_PreId = -2;
        ///// <summary>
        ///// 分选位流程托盘原ID
        ///// </summary>
        //public int Tray23_PreId
        //{
        //    get
        //    {
        //        if (tray23_PreId == -2)
        //        {
        //            tray23_PreId = Arthur.Utility._Convert.StrToInt(Arthur.Business.Application.GetOption("Tray23_PreId"), -1);
        //            if (tray23_PreId == -1)
        //            {
        //                tray23_PreId = 0;
        //                Arthur.Business.Application.SetOption("Tray23_PreId", tray23_PreId.ToString(), "分选位流程托盘原ID");
        //            }
        //        }
        //        return tray23_PreId;
        //    }
        //    set
        //    {
        //        if (tray23_PreId != value)
        //        {
        //            Arthur.Business.Application.SetOption("Tray23_PreId", value.ToString());
        //            SetProperty(ref tray23_PreId, value);
        //        }
        //    }
        //}

        #endregion

        #region 扫码就绪信号

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

        public bool isAlreadyUnbindTrayScan { get; set; }
        #endregion

    }
}
