using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Arthur.Utils
{
    public static class NetHelper
    {
        /// <summary>
        /// 获取本地电脑IP列表
        /// </summary>
        /// <returns></returns>
        public static List<IPAddress> GetLocalIPList()
        {
            string hostname = Dns.GetHostName();
            IPAddress[] ipadrarray = Dns.GetHostAddresses(hostname);
            List<IPAddress> ipadrslist = new List<IPAddress>();
            foreach (IPAddress ipa in ipadrarray)
            {
                if (ipa.AddressFamily == AddressFamily.InterNetwork)
                    ipadrslist.Add(ipa);
            }
            return ipadrslist;
        }
        public static byte GetIpLastValue(string ip)
        {
            var result = (byte)0;
            if (ip.Split('.').Length == 4)
            {
                result = Convert.ToByte(ip.Split('.')[3]);
            }
            return result;
        }

        public static string GetLocalIpByRegex(string iPAddressRegex)
        {
            var result = string.Empty;
            List<IPAddress> ips = GetLocalIPList().Where(i => Regex.IsMatch(i.ToString(), iPAddressRegex)).ToList();
            if (ips.Count > 0)
            {
                result = ips[0].ToString();
            }
            return result;
        }
    }
}
