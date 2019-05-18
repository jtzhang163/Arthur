﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Business
{
    public class OrderManage
    {
        /// <summary>
        /// 根据通道序号获取绑盘序号
        /// </summary>
        /// <param name="chargeOrder"></param>
        /// <returns></returns>
        public static int GetBindOrder(int chargeOrder)
        {
            var order = Data.OrderData.Orders.First(o => o.ChargeOrder == chargeOrder);
            if (order != null)
            {
                return order.BindOrder;
            }
            return 0;
        }


        /// <summary>
        /// 根据绑盘序号获取通道序号
        /// </summary>
        /// <param name="chargeOrder"></param>
        /// <returns></returns>
        public static int GetChargeOrder(int bindOrder)
        {
            var order = Data.OrderData.Orders.First(o => o.BindOrder == bindOrder);
            if (order != null)
            {
                return order.ChargeOrder;
            }
            return 0;
        }
    }
}
