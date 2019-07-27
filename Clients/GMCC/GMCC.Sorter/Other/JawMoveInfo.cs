using GMCC.Sorter.Model;
using GMCC.Sorter.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter
{
    /// <summary>
    /// 横移移动的信息（从PLC中读得的值）
    /// </summary>
    public sealed class JawMoveInfo
    {

        public TaskType Type { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public int Floor { get; set; }

        public static JawMoveInfo Create(TaskType type, StorageViewModel storage)
        {
            return new JawMoveInfo() { Floor = storage.Floor, Row = storage.Column / (Common.STOR_COL_COUNT / 2 + 1) + 1, Col = (storage.Column - 1) % (Common.STOR_COL_COUNT / 2) + 1, Type = type };
        }

        public override bool Equals(object obj)
        {
            var i = obj as JawMoveInfo;
            return this.Row == i.Row && this.Col == i.Col && this.Floor == i.Floor;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
