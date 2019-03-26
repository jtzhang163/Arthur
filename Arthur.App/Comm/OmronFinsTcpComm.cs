using Arthur.App.Model;
using HslCommunication;
using HslCommunication.Profinet.Omron;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Arthur.App.Comm
{
    public class OmronFinsTcpComm : IComm
    {
        public Result Connect(Commor commor)
        {
            commor.Connector = null;
            var omronFinsNet = new OmronFinsNet();
            var ethernetCommor = (EthernetCommor)commor.Communicator;
            try
            {
                omronFinsNet.ConnectTimeOut = 2000;
                omronFinsNet.IpAddress = ethernetCommor.IP;
                omronFinsNet.Port = ethernetCommor.Port;
                omronFinsNet.SA1 = 192;
                omronFinsNet.DA2 = 0;
                omronFinsNet.ByteTransform.DataFormat = (HslCommunication.Core.DataFormat)Enum.Parse(typeof(HslCommunication.Core.DataFormat), "ABCD");

                OperateResult connect = omronFinsNet.ConnectServer();
                if (connect.IsSuccess)
                {
                    commor.Connector = omronFinsNet;
                    return Result.OK;
                }
                else
                {
                    return new Result(string.Format("连接{0}失败({1} ：{2})", ethernetCommor.Name, ethernetCommor.IP, ethernetCommor.Port));
                }

            }
            catch (Exception ex)
            {
                return new Result(ex.Message);
            }
        }

        public Result EndConnect(Commor commor)
        {
            var omronFinsNet = (OmronFinsNet)commor.Connector;
            omronFinsNet.ConnectClose();
            return Result.OK;
        }

        public Result Comm(Commor commor, string input)
        {
            var omronFinsNet = (OmronFinsNet)commor.Connector;
            var ethernetCommor = (EthernetCommor)commor.Communicator;

            return Result.OK;
        }
    }
}
