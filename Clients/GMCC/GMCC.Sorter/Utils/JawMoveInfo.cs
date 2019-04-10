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
    public class JawMoveInfo
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public int Floor { get; set; }

        public static JawMoveInfo Create(TaskType type, StorageViewModel storage)
        {
            return new JawMoveInfo() { Floor = storage.Floor, Row = storage.Column / 10 + 1, Col = (storage.Column - 1) % 9 + 1 };
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
