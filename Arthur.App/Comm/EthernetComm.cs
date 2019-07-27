using Arthur.Core;
using Arthur.App.Business;
using Arthur.App.Model;
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
    public class EthernetComm : IComm
    {
        public Result Connect(Commor commor)
        {
            commor.Connector = null;
            var ethernetCommor = (EthernetCommor)commor.Communicator;
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                ConnectSocketDelegate connect = ConnectSocket;
                IAsyncResult asyncResult = connect.BeginInvoke(socket, ethernetCommor.IP, ethernetCommor.Port, null, null);
                bool success = asyncResult.AsyncWaitHandle.WaitOne(1000, false);
                if (!success)
                {
                    return new Result(string.Format("连接{0}失败({1} ：{2})", ethernetCommor.Name, ethernetCommor.IP, ethernetCommor.Port));
                }
                //socket.Connect(ethernetCommor.IP, ethernetCommor.Port); 连接失败时卡死 超时(20s)
            }
            catch (Exception ex)
            {
                return new Result(ex.Message);
            }
            if (!socket.Connected)
            {
                return new Result(string.Format("连接失败：{0}：{1}", ethernetCommor.IP, ethernetCommor.Port));
            }
            commor.Connector = socket;
            return Result.Success;
        }

        public Result EndConnect(Commor commor)
        {
            var socket = (Socket)commor.Connector;
            if (socket != null)
            {
                socket.Close();
            }
            return Result.Success;
        }

        public Result Comm(Commor commor, string input)
        {
            var socket = (Socket)commor.Connector;
            var ethernetCommor = (EthernetCommor)commor.Communicator;
            var recvData = string.Empty;
            try
            {

                if (socket == null || !socket.Connected)
                {
                    commor.Connect();
                    socket = (Socket)commor.Connector;
                }

                if (socket.Connected)
                {
                    var readtimeout = Application.EthernetReadTimeout;

                    Byte[] sendBytes = Encoding.UTF8.GetBytes(input + "\r");
                    socket.Send(sendBytes);

                    Thread.Sleep(10);

                    Byte[] Data = new Byte[1024];

                    if (readtimeout > 0)
                    {
                        Stopwatch sw = new Stopwatch();
                        var getStr = string.Empty;
                        var connectSuccess = false;
                        // Try to open the connection, if anything goes wrong, make sure we set connectSuccess = false
                        Thread t = new Thread(delegate ()
                        {
                            try
                            {
                                sw.Start();
                                socket.Receive(Data);
                                getStr = Encoding.ASCII.GetString(Data);
                                connectSuccess = true;
                            }
                            catch { }
                        });

                        // Make sure it's marked as a background thread so it'll get cleaned up automatically
                        t.IsBackground = true;
                        t.Start();

                        // Keep trying to join the thread until we either succeed or the timeout value has been exceeded
                        while (readtimeout > sw.ElapsedMilliseconds)
                            if (t.Join(1))
                                break;
                        // IsCommunicatting = false;
                        // If we didn't connect successfully, throw an exception
                        if (connectSuccess)
                        {
                            //throw new Exception("Timed out while trying to connect.");
                            recvData = getStr.Trim('\0', ' ','\r', '\n');
                        }
                        else
                        {
                            return new Result("读取数据超时：" + ethernetCommor.Name);
                        }
                    }
                    else
                    {
                        socket.Receive(Data);
                        recvData = Encoding.ASCII.GetString(Data).Trim('\0', ' ', '\r', '\n');
                    }
                }
                else
                {
                    throw new Exception("重复连接失败");
                }
            }
            catch (Exception ex)
            {
                var msg = string.Format("和{0}通信出现异常！原因：{1}", ethernetCommor.Name, ex.Message);
                return new Result(msg);
            }
            return Result.SuccessHasData(recvData);
        }

        private delegate string ConnectSocketDelegate(Socket socket, string ip, int port);
        private string ConnectSocket(Socket socket, string ip, int port)
        {
            string exmessage = "";
            try
            {
                socket.Connect(ip, port);
            }
            catch (Exception ex)
            {
                exmessage = ex.Message;
            }
            return exmessage;
        }
    }
}
