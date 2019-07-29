using Arthur.Core;
using Arthur.App.Model;
using HslCommunication.Profinet.Omron;
using System.IO.Ports;
using System.Net.Sockets;

namespace Arthur.App.Comm
{
    /// <summary>
    /// 通信连接抽象类
    /// </summary>
    public class Commor
    {
        /// <summary>
        /// 连接器
        /// </summary>
        public object Connector { get; set; }

        /// <summary>
        /// 通信器
        /// </summary>
        public Communicator Communicator { get; set; }

        public bool Connected { get; private set; }

        public Commor(Communicator communicator)
        {
            this.Communicator = communicator;
        }

        public Result Connect()
        {
            var result = new Result();

            if (!this.Connected)
            {
                if (this.Communicator.CommType == "Serial")
                {
                    result = new SerialComm().Connect(this);
                }
                else if (this.Communicator.CommType == "OmronFinsTcp")
                {
                    result = new OmronFinsTcpComm().Connect(this);
                }
                else if (this.Communicator.CommType == "Ethernet")
                {
                    result = new EthernetComm().Connect(this);
                }
            }

            this.Connected = result.IsSucceed;
            return result;
        }

        public Result EndConnect()
        {
            var result = new Result();
            if (this.Communicator.CommType == "Serial")
            {
                result = new SerialComm().EndConnect(this);
            }
            else if (this.Communicator.CommType == "OmronFinsTcp")
            {
                result = new OmronFinsTcpComm().EndConnect(this);
            }
            else if (this.Communicator.CommType == "Ethernet")
            {
                result = new EthernetComm().EndConnect(this);
            }
            Connected = false;
            return Result.Success;
        }

        public Result Comm(string input, int timeout)
        {

            if (this.Communicator.CommType == "Serial")
            {
                return new SerialComm().Comm(this, input, timeout);
            }
            else if (this.Communicator.CommType == "OmronFinsTcp")
            {
                return new OmronFinsTcpComm().Comm(this, input, timeout);
            }
            else if (this.Communicator.CommType == "Ethernet")
            {
                return new EthernetComm().Comm(this, input, timeout);
            }
            return new Result("连接为未知类型！");
        }

        public Result Read(string addr)
        {
            return Read(addr, (ushort)1);
        }

        public Result Read(string addr, ushort length)
        {
            if (this.Communicator.CommType == "OmronFinsTcp")
            {
                return new OmronFinsTcpComm().Read(this, addr, length);
            }
            return new Result("连接为未知类型！");
        }

        public Result ReadInt(string addr)
        {
            if (this.Communicator.CommType == "OmronFinsTcp")
            {
                return new OmronFinsTcpComm().ReadInt(this, addr);
            }
            return new Result("连接为未知类型！");
        }

        public Result Write(string addr, ushort value)
        {
            if (this.Communicator.CommType == "OmronFinsTcp")
            {
                return new OmronFinsTcpComm().Write(this, addr, value);
            }
            return new Result("连接为未知类型！");
        }
    }
}
