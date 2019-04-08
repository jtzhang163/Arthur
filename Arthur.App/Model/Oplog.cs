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
    /// 操作日志
    /// </summary>
    public class Oplog : Logging
    {

        public Oplog() : this(-1)
        {

        }

        public Oplog(int id)
        {
            Id = id;
        }


        #region 属性

        /// <summary>
        /// 操作内容
        /// </summary>
        [MaxLength(255)]
        public string Content { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public OpType OpType { get; set; }

        #endregion
    }

    
    public enum OpType
    {
        创建 = 1,
        编辑 = 2,
        删除 = 3
    }
}
