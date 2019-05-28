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
    /// Neware共享数据库上下文
    /// </summary>
    public class NewareContext : DbContext
    {
        public NewareContext() : base(Current.Option.NewareConnString)
        {
        }
    }
}
