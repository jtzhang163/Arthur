using Arthur.App.Model;
using System.IO.Ports;
using System.Net.Sockets;

namespace Arthur.App.Comm
{
    /// <summary>
    /// 通信连接抽象类
    /// </summary>
    public class Commor
    {

        public object Connector { get; set; }

        public Communicator Communicator { get; set; }

        public bool Connected { get; private set; }

        public Commor(Communicator communicator)
        {
            this.Communicator = communicator;
            if(this.Communicator is SerialCommor)
            {
                var serialCommor = (SerialCommor)this.Communicator;
                this.Connector = new SerialPort(serialCommor.PortName, serialCommor.BaudRate, serialCommor.Parity, serialCommor.DataBits, serialCommor.StopBits);
            }
            else if (this.Communicator.Company == "OMRON" && this.Communicator is EthernetCommor)
            {

            }
            else if (this.Communicator is EthernetCommor)
            {
                var ethernetCommor = (EthernetCommor)this.Communicator;
                this.Connector = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
        }

        public Result Connect()
        {
            var result = new Result();
            if (this.Communicator is SerialCommor)
            {
                result = Communicate.SerialConnect(this);
            }
            else if (this.Communicator.Company == "OMRON" && this.Communicator is EthernetCommor)
            {

            }
            else if (this.Communicator is EthernetCommor)
            {
                result = Communicate.EthernetConnect(this);
            }

            this.Connected = result.IsOk;
            return result;
        }

        public Result EndConnect()
        {
            var result = new Result();
            if (this.Communicator is SerialCommor)
            {
                result = Communicate.SerialEndConnect(this);
            }
            else if (this.Communicator.Company == "OMRON" && this.Communicator is EthernetCommor)
            {

            }
            else if (this.Communicator is EthernetCommor)
            {
                result = Communicate.EthernetEndConnect(this);
            }
            Connected = false;
            return Result.OK;
        }

        public Result Comm(string input)
        {

            if (this.Communicator is SerialCommor)
            {
                return Communicate.SerialComm(this, input);

            }
            else if (this.Communicator.Company == "OMRON" && this.Communicator is EthernetCommor)
            {

            }
            else if (this.Communicator is EthernetCommor)
            {
                return Communicate.EthernetComm(this, input);
            }
            return new Result("连接为未知类型！");
        }

    }
}
