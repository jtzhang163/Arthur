using Arthur.Core;
using GMCC.Sorter.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Data
{
    /// <summary>
    /// 共享数据库上下文
    /// </summary>
    public class ShareContext : DbContext
    {
        public ShareContext() : base(Current.Option.ShareConnString)
        {
        }
    }
}
