﻿using Arthur.Core;
using Arthur.App;
using GMCC.Sorter.Extensions;
using GMCC.Sorter.Model;
using GMCC.Sorter.Other;
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
    public sealed class AppOption : BindableObject
    {

        public string Name = "配置";

        private string newareConnString;
        public string NewareConnString
        {
            get
            {
                if (newareConnString == null)
                {
                    newareConnString = ConfigurationManager.ConnectionStrings["NewareShareDB"].ToString();
                }
                return newareConnString;
            }
        }

        private string gmccConnString;
        public string GMCCConnString
        {
            get
            {
                if (gmccConnString == null)
                {
                    gmccConnString = ConfigurationManager.ConnectionStrings["GMCCShareDB"].ToString();
                }
                return gmccConnString;
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
                    taskExecInterval = Arthur.Core.Transfer._Convert.To(Arthur.App.Business.Setting.GetOption("TaskExecInterval"), -1);
                    if (taskExecInterval < 0)
                    {
                        taskExecInterval = 3000;
                        Arthur.App.Business.Setting.SetOption("TaskExecInterval", taskExecInterval.ToString(), "搬运任务执行定时器间隔(ms)");
                    }
                }
                return taskExecInterval;
            }
            set
            {
                if (taskExecInterval != value)
                {
                    Arthur.App.Business.Setting.SetOption("TaskExecInterval", value.ToString());
                    Arthur.App.Business.Logging.AddOplog(string.Format("配置. 搬运任务执行定时器间隔(ms): [{0}] 修改为 [{1}]", taskExecInterval, value), Arthur.App.Model.OpType.编辑);
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
                    getShareDataExecInterval = Arthur.Core.Transfer._Convert.To(Arthur.App.Business.Setting.GetOption("GetShareDataExecInterval"), -1);
                    if (getShareDataExecInterval < 0)
                    {
                        getShareDataExecInterval = 3000;
                        Arthur.App.Business.Setting.SetOption("GetShareDataExecInterval", getShareDataExecInterval.ToString(), "获取共享数据执行定时器间隔(ms)");
                    }
                }
                return getShareDataExecInterval;
            }
            set
            {
                if (getShareDataExecInterval != value)
                {
                    Arthur.App.Business.Setting.SetOption("GetShareDataExecInterval", value.ToString());
                    Arthur.App.Business.Logging.AddOplog(string.Format("配置. 获取共享数据执行定时器间隔(ms): [{0}] 修改为 [{1}]", getShareDataExecInterval, value), Arthur.App.Model.OpType.编辑);
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
                    stillTimeSpan = Arthur.Core.Transfer._Convert.To(Arthur.App.Business.Setting.GetOption("StillTimeSpan"), -1);
                    if (stillTimeSpan == -1)
                    {
                        stillTimeSpan = 120;
                        Arthur.App.Business.Setting.SetOption("StillTimeSpan", stillTimeSpan.ToString(), "静置时长(min)");
                    }
                }
                return stillTimeSpan;
            }
            set
            {
                if (stillTimeSpan != value)
                {
                    Arthur.App.Business.Setting.SetOption("StillTimeSpan", value.ToString());
                    Arthur.App.Business.Logging.AddOplog(string.Format("设置. {0} 静置时间: [{1}] 修改为 [{2}]", Name, stillTimeSpan, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref stillTimeSpan, value);

                    //同步设置所有料仓静置时间
                    Current.Storages.ForEach(o => o.StillTimeSpan = value);
                }
            }
        }


        private int currentPackOrder = -2;
        /// <summary>
        /// 当前分选托盘打包次序
        /// 1-32
        /// </summary>
        public int CurrentPackOrder
        {
            get
            {
                if (currentPackOrder == -2)
                {
                    currentPackOrder = Arthur.Core.Transfer._Convert.To(Arthur.App.Business.Setting.GetOption("CurrentPackOrder"), -1);
                    if (currentPackOrder == -1)
                    {
                        currentPackOrder = 1;
                        Arthur.App.Business.Setting.SetOption("CurrentPackOrder", currentPackOrder.ToString(), "当前分选托盘打包次序");
                    }
                }
                return currentPackOrder;
            }
            set
            {
                if (currentPackOrder != value)
                {
                    Arthur.App.Business.Setting.SetOption("CurrentPackOrder", value.ToString());
                    Arthur.App.Business.Logging.AddOplog(string.Format("设置. {0} 当前分选托盘打包次序: [{1}] 修改为 [{2}]", Name, currentPackOrder, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref currentPackOrder, value);
                }
            }
        }



        private int lastFeedTaskStorageColumn = -2;
        /// <summary>
        /// 上个上料任务料仓的列序号
        /// </summary>
        public int LastFeedTaskStorageColumn
        {
            get
            {
                if (lastFeedTaskStorageColumn == -2)
                {
                    lastFeedTaskStorageColumn = Arthur.Core.Transfer._Convert.To(Arthur.App.Business.Setting.GetOption("LastFeedTaskStorageColumn"), -1);
                    if (lastFeedTaskStorageColumn == -1)
                    {
                        lastFeedTaskStorageColumn = 0;
                        Arthur.App.Business.Setting.SetOption("LastFeedTaskStorageColumn", lastFeedTaskStorageColumn.ToString(), "上个上料任务料仓的列序号");
                    }
                }
                return lastFeedTaskStorageColumn;
            }
            set
            {
                if (lastFeedTaskStorageColumn != value)
                {
                    Arthur.App.Business.Setting.SetOption("LastFeedTaskStorageColumn", value.ToString());
                    Arthur.App.Business.Logging.AddOplog(string.Format("设置. {0} 上个上料任务料仓的列序号: [{1}] 修改为 [{2}]", Name, lastFeedTaskStorageColumn, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref lastFeedTaskStorageColumn, value);
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

        public bool IsGetShareDataExecting { get; set; }


        private TaskPriorityType taskPriorityType = TaskPriorityType.未知;
        /// <summary>
        /// 自动任务优先级类型（层优先/列优先）
        /// </summary>
        public TaskPriorityType TaskPriorityType
        {
            get
            {
                if (taskPriorityType == TaskPriorityType.未知)
                {
                    var type = Arthur.App.Business.Setting.GetOption("TaskPriorityType");
                    if (type == null)
                    {
                        taskPriorityType = TaskPriorityType.层优先;
                        Arthur.App.Business.Setting.SetOption("TaskPriorityType", taskPriorityType.ToString(), "自动任务优先级类型（层优先/列优先）");
                    }
                    else
                    {
                        taskPriorityType = (TaskPriorityType)Enum.Parse(typeof(TaskPriorityType), type);
                    }
                }
                return taskPriorityType;
            }
            set
            {
                if (taskPriorityType != value)
                {
                    Arthur.App.Business.Setting.SetOption("TaskPriorityType", value.ToString());
                    Arthur.App.Business.Logging.AddOplog(string.Format("设置. {0} 自动任务优先级类型（层优先/列优先）: [{1}] 修改为 [{2}]", Name, taskPriorityType, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref taskPriorityType, value);
                }
            }
        }


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
                    jawProcTrayId = Arthur.Core.Transfer._Convert.To(Arthur.App.Business.Setting.GetOption("JawProcTrayId"), -1);
                    if (jawProcTrayId == -1)
                    {
                        jawProcTrayId = 0;
                        Arthur.App.Business.Setting.SetOption("JawProcTrayId", jawProcTrayId.ToString(), "横移流程托盘Id");
                    }
                }
                return jawProcTrayId;
            }
            set
            {
                if (jawProcTrayId != value)
                {
                    Arthur.App.Business.Setting.SetOption("JawProcTrayId", value.ToString());
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
                    jawPos = Arthur.Core.Transfer._Convert.To(Arthur.App.Business.Setting.GetOption("JawPos"), -1);
                    if (jawPos == -1)
                    {
                        jawPos = 0;
                        Arthur.App.Business.Setting.SetOption("JawPos", jawPos.ToString(), "横移位置");
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
                    Arthur.App.Business.Setting.SetOption("JawPos", value.ToString());
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

                if (isHasTray11.HasValue && isHasTray11 != value)
                {
                    if (value)
                    {
                        LogHelper.WriteInfo("****************************** 绑盘位 托盘信号：无--->有 ****************************");
                    }
                    else
                    {
                        LogHelper.WriteInfo("****************************** 绑盘位 托盘信号：有--->无 ****************************");
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


                if (isHasTray12.HasValue && isHasTray12 != value)
                {
                    if (value)
                    {
                        LogHelper.WriteInfo("****************************** 充电位 托盘信号：无--->有 ****************************");
                    }
                    else
                    {
                        LogHelper.WriteInfo("****************************** 充电位 托盘信号：有--->无 ****************************");
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

                if (isHasTray13.HasValue && isHasTray13 != value)
                {
                    if (value)
                    {
                        LogHelper.WriteInfo("****************************** 上料位 托盘信号：无--->有 ****************************");
                    }
                    else
                    {
                        LogHelper.WriteInfo("****************************** 上料位 托盘信号：有--->无 ****************************");
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

                if (isHasTray21.HasValue && isHasTray21 != value)
                {
                    if (value)
                    {
                        LogHelper.WriteInfo("****************************** 下料位 托盘信号：无--->有 ****************************");
                    }
                    else
                    {
                        LogHelper.WriteInfo("****************************** 下料位 托盘信号：有--->无 ****************************");
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

                if (isHasTray22.HasValue && isHasTray22 != value)
                {
                    if (value)
                    {
                        LogHelper.WriteInfo("****************************** 放电位 托盘信号：无--->有 ****************************");
                    }
                    else
                    {
                        LogHelper.WriteInfo("****************************** 放电位 托盘信号：有--->无 ****************************");
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
                        this.Tray23_PreId = this.Tray23_Id;
                        this.Tray23_Id = 0;
                    }
                }

                if (isHasTray23.HasValue && isHasTray23 != value)
                {
                    if (value)
                    {
                        LogHelper.WriteInfo("****************************** 分选位 托盘信号：无--->有 ****************************");
                    }
                    else
                    {
                        LogHelper.WriteInfo("****************************** 分选位 托盘信号：有--->无 ****************************");
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
                    tray11_Id = Arthur.Core.Transfer._Convert.To(Arthur.App.Business.Setting.GetOption("Tray11_Id"), -1);
                    if (tray11_Id == -1)
                    {
                        tray11_Id = 0;
                        Arthur.App.Business.Setting.SetOption("Tray11_Id", tray11_Id.ToString(), "绑盘位流程托盘Id");
                    }
                }
                return tray11_Id;
            }
            set
            {
                if (tray11_Id != value)
                {
                    Arthur.App.Business.Setting.SetOption("Tray11_Id", value.ToString());
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
                    tray12_Id = Arthur.Core.Transfer._Convert.To(Arthur.App.Business.Setting.GetOption("Tray12_Id"), -1);
                    if (tray12_Id == -1)
                    {
                        tray12_Id = 0;
                        Arthur.App.Business.Setting.SetOption("Tray12_Id", tray12_Id.ToString(), "充电位流程托盘Id");
                    }
                }
                return tray12_Id;
            }
            set
            {
                if (tray12_Id != value)
                {
                    Arthur.App.Business.Setting.SetOption("Tray12_Id", value.ToString());
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
                    tray13_Id = Arthur.Core.Transfer._Convert.To(Arthur.App.Business.Setting.GetOption("Tray13_Id"), -1);
                    if (tray13_Id == -1)
                    {
                        tray13_Id = 0;
                        Arthur.App.Business.Setting.SetOption("Tray13_Id", tray13_Id.ToString(), "上料位流程托盘Id");
                    }
                }
                return tray13_Id;
            }
            set
            {
                if (tray13_Id != value)
                {
                    Arthur.App.Business.Setting.SetOption("Tray13_Id", value.ToString());
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
                    tray21_Id = Arthur.Core.Transfer._Convert.To(Arthur.App.Business.Setting.GetOption("Tray21_Id"), -1);
                    if (tray21_Id == -1)
                    {
                        tray21_Id = 0;
                        Arthur.App.Business.Setting.SetOption("Tray21_Id", tray21_Id.ToString(), "下料位流程托盘Id");
                    }
                }
                return tray21_Id;
            }
            set
            {
                if (tray21_Id != value)
                {
                    Arthur.App.Business.Setting.SetOption("Tray21_Id", value.ToString());
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
                    tray22_Id = Arthur.Core.Transfer._Convert.To(Arthur.App.Business.Setting.GetOption("Tray22_Id"), -1);
                    if (tray22_Id == -1)
                    {
                        tray22_Id = 0;
                        Arthur.App.Business.Setting.SetOption("Tray22_Id", tray22_Id.ToString(), "放电位流程托盘Id");
                    }
                }
                return tray22_Id;
            }
            set
            {
                if (tray22_Id != value)
                {
                    Arthur.App.Business.Setting.SetOption("Tray22_Id", value.ToString());
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
                    tray23_Id = Arthur.Core.Transfer._Convert.To(Arthur.App.Business.Setting.GetOption("Tray23_Id"), -1);
                    if (tray23_Id == -1)
                    {
                        tray23_Id = 0;
                        Arthur.App.Business.Setting.SetOption("Tray23_Id", tray23_Id.ToString(), "分选位流程托盘Id");
                    }
                }
                return tray23_Id;
            }
            set
            {
                if (tray23_Id != value)
                {
                    Arthur.App.Business.Setting.SetOption("Tray23_Id", value.ToString());
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
                    tray11_PreId = Arthur.Core.Transfer._Convert.To(Arthur.App.Business.Setting.GetOption("Tray11_PreId"), -1);
                    if (tray11_PreId == -1)
                    {
                        tray11_PreId = 0;
                        Arthur.App.Business.Setting.SetOption("Tray11_PreId", tray11_PreId.ToString(), "绑盘位流程托盘原ID");
                    }
                }
                return tray11_PreId;
            }
            set
            {
                if (tray11_PreId != value)
                {
                    Arthur.App.Business.Setting.SetOption("Tray11_PreId", value.ToString());
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
                    tray12_PreId = Arthur.Core.Transfer._Convert.To(Arthur.App.Business.Setting.GetOption("Tray12_PreId"), -1);
                    if (tray12_PreId == -1)
                    {
                        tray12_PreId = 0;
                        Arthur.App.Business.Setting.SetOption("Tray12_PreId", tray12_PreId.ToString(), "充电位流程托盘原ID");
                    }
                }
                return tray12_PreId;
            }
            set
            {
                if (tray12_PreId != value)
                {
                    Arthur.App.Business.Setting.SetOption("Tray12_PreId", value.ToString());
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
        //            tray13_PreId = Arthur.Core.Transfer._Convert.To(Arthur.App.Business.Application.GetOption("Tray13_PreId"), -1);
        //            if (tray13_PreId == -1)
        //            {
        //                tray13_PreId = 0;
        //                Arthur.App.Business.Application.SetOption("Tray13_PreId", tray13_PreId.ToString(), "上料位流程托盘原ID");
        //            }
        //        }
        //        return tray13_PreId;
        //    }
        //    set
        //    {
        //        if (tray13_PreId != value)
        //        {
        //            Arthur.App.Business.Application.SetOption("Tray13_PreId", value.ToString());
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
                    tray21_PreId = Arthur.Core.Transfer._Convert.To(Arthur.App.Business.Setting.GetOption("Tray21_PreId"), -1);
                    if (tray21_PreId == -1)
                    {
                        tray21_PreId = 0;
                        Arthur.App.Business.Setting.SetOption("Tray21_PreId", tray21_PreId.ToString(), "下料位流程托盘原ID");
                    }
                }
                return tray21_PreId;
            }
            set
            {
                if (tray21_PreId != value)
                {
                    Arthur.App.Business.Setting.SetOption("Tray21_PreId", value.ToString());
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
                    tray22_PreId = Arthur.Core.Transfer._Convert.To(Arthur.App.Business.Setting.GetOption("Tray22_PreId"), -1);
                    if (tray22_PreId == -1)
                    {
                        tray22_PreId = 0;
                        Arthur.App.Business.Setting.SetOption("Tray22_PreId", tray22_PreId.ToString(), "放电位流程托盘原ID");
                    }
                }
                return tray22_PreId;
            }
            set
            {
                if (tray22_PreId != value)
                {
                    Arthur.App.Business.Setting.SetOption("Tray22_PreId", value.ToString());
                    SetProperty(ref tray22_PreId, value);
                }
            }
        }


        private int tray23_PreId = -2;
        /// <summary>
        /// 分选位流程托盘原ID
        /// </summary>
        public int Tray23_PreId
        {
            get
            {
                if (tray23_PreId == -2)
                {
                    tray23_PreId = Arthur.Core.Transfer._Convert.To(Arthur.App.Business.Setting.GetOption("Tray23_PreId"), -1);
                    if (tray23_PreId == -1)
                    {
                        tray23_PreId = 0;
                        Arthur.App.Business.Setting.SetOption("Tray23_PreId", tray23_PreId.ToString(), "分选位流程托盘原ID");
                    }
                }
                return tray23_PreId;
            }
            set
            {
                if (tray23_PreId != value)
                {
                    Arthur.App.Business.Setting.SetOption("Tray23_PreId", value.ToString());
                    SetProperty(ref tray23_PreId, value);
                }
            }
        }

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
                    IsAlreadyUnbindTrayScan = false;
                }
                SetProperty(ref isUnbindTrayScanReady, value);
            }
        }


        public bool IsAlreadyBatteryScan { get; set; }
        public bool IsAlreadyBindTrayScan { get; set; }
        public bool IsAlreadyUnbindTrayScan { get; set; }
        #endregion


        private string productModel;

        /// <summary>
        /// 当前产品型号
        /// </summary>
        public string ProductModel
        {
            get
            {
                if (string.IsNullOrEmpty(productModel))
                {
                    productModel = Arthur.App.Business.Setting.GetOption("ProductModel");
                    if (string.IsNullOrEmpty(productModel))
                    {
                        productModel = "unknown";
                        Arthur.App.Business.Setting.SetOption("ProductModel", productModel, "当前产品型号");
                    }
                }
                return productModel;
            }
            set
            {
                if (productModel != value)
                {
                    Arthur.App.Business.Setting.SetOption("ProductModel", value);
                    Arthur.App.Business.Logging.AddOplog(string.Format("配置. 当前产品型号: [{0}] 修改为 [{1}]", productModel, value), Arthur.App.Model.OpType.编辑);
                    this.SetProperty(ref productModel, value);
                }
            }
        }

    }
}
