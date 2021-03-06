﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMCC.Sorter.Other;

namespace GMCC.Sorter.ViewModel
{
    public sealed class AppViewModel : Arthur.App.ViewModel.AppViewModel
    {

        private string appName = null;

        public string AppName
        {
            get
            {
                if (appName == null)
                {
                    if (string.IsNullOrWhiteSpace(Arthur.App.Current.Option.AppName))
                    {
                        Arthur.App.Current.Option.AppName = "烯晶碳能分选机调度系统";
                    }
                    appName = Arthur.App.Current.Option.AppName;
                }
                return appName;
            }
            set
            {
                if (appName != value)
                {
                    Arthur.App.Current.Option.AppName = value;

                    //更新AppName时同步更新AppTitle，使窗体标题栏变化
                    AppTitle = AppNameToTitle(value);

                    Arthur.App.Business.Logging.AddOplog(string.Format("系统参数. 程序名称: [{0}] 修改为 [{1}]", appName, value), Arthur.App.Model.OpType.编辑);

                    SetProperty(ref appName, value);
                }
            }
        }

        private string appTitle = null;

        public string AppTitle
        {
            get
            {
                if (appTitle == null)
                {
                    appTitle = AppNameToTitle(AppName);
                }
                return appTitle;
            }
            set
            {
                if (appTitle != value)
                {
                    SetProperty(ref appTitle, value);
                }
            }
        }

        private string AppNameToTitle(string appName)
        {
            var appInfo = new AppInfo();
            return string.Format("{0} {1}   {2}", appName, appInfo.VersionInfo, appInfo.UpdateTime.ToString("yyyy/M/d"));
        }


        public string Version
        {
            get
            {
                var appInfo = new AppInfo();
                return appInfo.VersionInfo.TrimStart('V');
            }
        }

        private string expireTip = null;

        public string ExpireTip
        {
            get
            {
                return expireTip;
            }
            set
            {
                if (expireTip != value)
                {
                    SetProperty(ref expireTip, value);
                }
            }
        }

        private string companyName = null;

        public string CompanyName
        {
            get
            {
                if (companyName == null)
                {
                    if (string.IsNullOrWhiteSpace(Arthur.App.Current.Option.CompanyName))
                    {
                        Arthur.App.Current.Option.CompanyName = "XXXXXXXXX公司";
                    }
                    companyName = Arthur.App.Current.Option.CompanyName;
                }
                return companyName;
            }
            set
            {
                if (companyName != value)
                {
                    Arthur.App.Current.Option.CompanyName = value;


                    Arthur.App.Business.Logging.AddOplog(string.Format("系统参数. 公司名: [{0}] 修改为 [{1}]", companyName, value), Arthur.App.Model.OpType.编辑);

                    SetProperty(ref companyName, value);
                }
            }
        }

        private RunStatus runStatus = RunStatus.闲置;
        public RunStatus RunStatus
        {
            get => runStatus;
            set
            {
                if (runStatus != value)
                {
                    var selectedEnabled = value == RunStatus.闲置;
                    Factory.CommorFactory.GetCommors().ForEach(c => c.SelectedEnabled = selectedEnabled);
                    SetProperty(ref runStatus, value);
                }
            }
        }

        private TaskMode taskMode = TaskMode.手动任务;
        public TaskMode TaskMode
        {
            get => taskMode;
            set
            {
                SetProperty(ref taskMode, value);
            }
        }


        private string errorMsg;
        public string ErrorMsg
        {
            get => errorMsg;
            set
            {
                SetProperty(ref errorMsg, value);
            }
        }

        public bool IsTerminalInitFinished { get; set; }
    }

    public enum RunStatus
    {
        未知 = 0,
        闲置 = 1,
        运行 = 2,
        停止 = 3,
        异常 = 4
    }

    public enum TaskMode
    {
        未知 = 0,
        手动任务 = 1,
        自动任务 = 2
    }
}
