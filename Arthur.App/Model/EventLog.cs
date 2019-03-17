using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.App.Model
{
    /// <summary>
    /// 系统事件日志
    /// </summary>
    public class EventLog : Service
    {

        public EventLog() : this(-1)
        {

        }

        public EventLog(int id)
        {
            Id = id;
        }


        #region 属性

        /// <summary>
        /// 事件内容
        /// </summary>
        [MaxLength(255)]
        public string Content { get; set; }

        public EventType EventType { get; set; }

        public DateTime Time { get; set; }

        #endregion
    }

    public enum EventType
    {
        信息 = 1,
        警告 = 2,
        错误 = 3
    }

}
