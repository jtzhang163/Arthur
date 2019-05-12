using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.App
{
    public class CacheItem
    {
        public string Key { get; set; }

        public object Value { get; set; }

        public DateTime CreateTime { get; set; }

        public TimeSpan ExpireTime { get; set; }
    }
}
