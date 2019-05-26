using Arthur.Core;
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
            var omronFinsNet = new OmronFinsNet();
            var ethernetCommor = (EthernetCommor)commor.Communicator;
            try
            {
                omronFinsNet.ConnectTimeOut = 2000;
                omronFinsNet.IpAddress = ethernetCommor.IP;
                omronFinsNet.Port = ethernetCommor.Port;
                omronFinsNet.SA1 = NetHelper.GetIpLastValue(NetHelper.GetLocalIpByRegex("192.168.1.*")); ;
                omronFinsNet.DA2 = 0;
                omronFinsNet.ByteTransform.DataFormat = (HslCommunication.Core.DataFormat)Enum.Parse(typeof(HslCommunication.Core.DataFormat), "CDAB");

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
            if (omronFinsNet != null)
            {
                omronFinsNet.ConnectClose();
                commor.Connector = null;
            }
            return Result.OK;
        }

        public Result Comm(Commor commor, string input)
        {
            var omronFinsNet = (OmronFinsNet)commor.Connector;
            var ethernetCommor = (EthernetCommor)commor.Communicator;

            return Result.OK;
        }

        public Result Read(Commor commor, string addr, ushort length)
        {
            try
            {
                var omronFinsNet = (OmronFinsNet)commor.Connector;
                OperateResult<ushort[]> result = omronFinsNet.ReadUInt16(addr, length);

                if (result.IsSuccess)
                {
                    return Result.OkHasData(result.Content);
                }
                else
                {
                    return new Result(string.Format("{0}:{1}", result.ErrorCode, result.Message));
                }
            }
            catch (Exception ex)
            {
                return new Result(ex.Message);
            }
        }

        public Result ReadInt(Commor commor, string addr)
        {
            try
            {
                var omronFinsNet = (OmronFinsNet)commor.Connector;
                OperateResult<int> result = omronFinsNet.ReadInt32(addr);

                if (result.IsSuccess)
                {
                    return Result.OkHasData(result.Content);
                }
                else
                {
                    return new Result(string.Format("{0}:{1}", result.ErrorCode, result.Message));
                }
            }
            catch (Exception ex)
            {
                return new Result(ex.Message);
            }
        }

        public Result Write(Commor commor, string addr, ushort value)
        {
            try
            {
                var omronFinsNet = (OmronFinsNet)commor.Connector;
                OperateResult result = omronFinsNet.Write(addr, value);

                if (result.IsSuccess)
                {
                    return Result.OK;
                }
                else
                {
                    return new Result(string.Format("{0}:{1}", result.ErrorCode, result.Message));
                }
            }
            catch (Exception ex)
            {
                return new Result(ex.Message);
            }
        }
    }
}
