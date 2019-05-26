using Arthur.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.App.Model
{
    public abstract class Logging : Service
    {
        public int UserId { get; set; }

        public DateTime Time { get; set; } = Default.DateTime;
    }
}
