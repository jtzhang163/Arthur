using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter
{
    public class AppOption : Arthur.App.AppOption
    {
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


    }
}
