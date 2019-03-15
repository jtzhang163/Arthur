using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur
{
    public class LogHelper
    {

        private static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");

        private static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");
        /// <summary>
        /// 提示信息写入日志
        /// </summary>
        /// <param name="info">提示信息</param>
        public static void WriteInfo(string info)
        {

            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info(info);
            }
        }

        /// <summary>
        /// 异常信息写入日志
        /// </summary>
        /// <param name="error"></param>
        public static void WriteError(string error)
        {
            if (logerror.IsErrorEnabled)
            {
                logerror.Error(error);
            }
        }
        /// <summary>
        /// 异常信息写入日志
        /// </summary>
        /// <param name="ex"></param>
        public static void WriteError(Exception ex)
        {
            if (logerror.IsErrorEnabled)
            {
                logerror.Error("Error", ex);
            }
        }
    }
}
