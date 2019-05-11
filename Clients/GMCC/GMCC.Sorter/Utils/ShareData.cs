using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Utils
{
    public class ShareData
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public int Status { get; set; }
        public string Desc { get; set; }
        public int ProcTrayId { get; set; }
        public DateTime UpdateTime { get; set; }
    }

    public class BindCode
    {
        public string TrayCode { get; set; }
        public string BatteryCodes { get; set; }
    }

    public class SortingResult
    {
        public string TrayCode { get; set; }
        public string Results { get; set; }
    }
}
