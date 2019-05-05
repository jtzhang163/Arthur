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
            if(this.Communicator is SerialCommor)
            {
                var serialCommor = (SerialCommor)this.Communicator;
            }
            else if (this.Communicator.Company == "OMRON" && this.Communicator.ModelNumber == "CJ2M-CP33" && this.Communicator is EthernetCommor)
            {
                var ethernetCommor = (EthernetCommor)this.Communicator;
            }
            else if (this.Communicator is EthernetCommor)
            {
                var ethernetCommor = (EthernetCommor)this.Communicator;
            }
        }

        public Result Connect()
        {
            var result = new Result();
            if (this.Communicator is SerialCommor)
            {
                result = new SerialComm().Connect(this);
            }
            else if (this.Communicator.Company == "OMRON" && this.Communicator.ModelNumber == "CJ2M-CP33" && this.Communicator is EthernetCommor)
            {
                result = new OmronFinsTcpComm().Connect(this);
            }
            else if (this.Communicator is EthernetCommor)
            {
                result = new EthernetComm().Connect(this);
            }

            this.Connected = result.IsOk;
            return result;
        }

        public Result EndConnect()
        {
            var result = new Result();
            if (this.Communicator is SerialCommor)
            {
                result = new SerialComm().EndConnect(this);
            }
            else if (this.Communicator.Company == "OMRON" && this.Communicator.ModelNumber == "CJ2M-CP33" && this.Communicator is EthernetCommor)
            {
                result = new OmronFinsTcpComm().EndConnect(this);
            }
            else if (this.Communicator is EthernetCommor)
            {
                result = new EthernetComm().EndConnect(this);
            }
            Connected = false;
            return Result.OK;
        }

        public Result Comm(string input)
        {

            if (this.Communicator is SerialCommor)
            {
                return new SerialComm().Comm(this, input);
            }
            else if (this.Communicator.Company == "OMRON" && this.Communicator.ModelNumber == "CJ2M-CP33" && this.Communicator is EthernetCommor)
            {
                return new OmronFinsTcpComm().Comm(this, input);
            }
            else if (this.Communicator is EthernetCommor)
            {
                return new EthernetComm().Comm(this, input);
            }
            return new Result("连接为未知类型！");
        }

        public Result Read(string addr)
        {
            return Read(addr, (ushort)1);
        }

        public Result Read(string addr, ushort length)
        {
            if (this.Communicator.Company == "OMRON" && this.Communicator.ModelNumber == "CJ2M-CP33" && this.Communicator is EthernetCommor)
            {
                return new OmronFinsTcpComm().Read(this, addr, length);
            }
            return new Result("连接为未知类型！");
        }

        public Result ReadInt(string addr)
        {
            if (this.Communicator.Company == "OMRON" && this.Communicator.ModelNumber == "CJ2M-CP33" && this.Communicator is EthernetCommor)
            {
                return new OmronFinsTcpComm().ReadInt(this, addr);
            }
            return new Result("连接为未知类型！");
        }

        public Result Write(string addr, ushort value)
        {
            if (this.Communicator.Company == "OMRON" && this.Communicator.ModelNumber == "CJ2M-CP33" && this.Communicator is EthernetCommor)
            {
                return new OmronFinsTcpComm().Write(this, addr, value);
            }
            return new Result("连接为未知类型！");
        }
    }
}
