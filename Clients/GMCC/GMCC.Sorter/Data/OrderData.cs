using GMCC.Sorter.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Data
{
    /// <summary>
    /// 绑盘序号和通道序号关系数据
    /// </summary>
    public class OrderData
    {

        private static List<TrayOrder> orders;

        public static List<TrayOrder> Orders
        {
            get
            {
                if (orders == null)
                {
                    orders = new List<TrayOrder>();

                    for (var i = 1; i <= 8; i++)
                    {
                        for (var j = 1; j <= 4; j++)
                        {
                            orders.Add(new TrayOrder()
                            {
                                SortOrder = (i - 1) * 4 + j,
                                BindOrder = (i - 1) * 4 + (Common.PROJ_NO == "0079" ? 5 - j : j),
                                ChargeOrder = (5 - j) * 8 - i + 1,
                                //PackOrder = (i - 1) * 4 + (j % 4 < 2 ? j : j % 4 == 2 ? j + 1 : j - 1)
                                PackOrder = (i - 1) * 4 + j
                            });
                        }
                    }

                }
                return orders;
            }
        }
    }
}
