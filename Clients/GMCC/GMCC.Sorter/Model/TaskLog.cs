using Arthur.Core;
using Arthur.App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Model
{
    public sealed class TaskLog : Logging
    {
        public TaskType TaskType { get; set; }

        public int ProcTrayId { get; set; }

        public int StorageId { get; set; }

        public DateTime StartTime { get; set; } = Default.DateTime;

        public DateTime EndTime { get; set; } = Default.DateTime;
    }
}
