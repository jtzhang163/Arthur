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

        private static List<BindChargeOrder> orders;

        public static List<BindChargeOrder> Orders
        {
            get
            {
                if (orders == null)
                {
                    orders = new List<BindChargeOrder>();

                    for (var i = 1; i <= 8; i++)
                    {
                        for (var j = 1; j <= 4; j++)
                        {
                            orders.Add(new BindChargeOrder()
                            {
                                BindOrder = (i - 1) * 4 + 5 - j,
                                ChargeOrder = (5 - j) * 8 - i + 1
                            });
                        }
                    }

                }
                return orders;
            }
        }
    }
}
