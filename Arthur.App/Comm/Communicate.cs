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
    public static class Communicate
    {
        public static Result SerialConnect(Commor commor)
        {
            var serialPort = (SerialPort)commor.Connector;
            try
            {
                if (!serialPort.IsOpen)
                {
                    serialPort.Open();
                }
            }
            catch (Exception ex)
            {
                return new Result(ex.Message);
            }
            return Result.OK;
        }

        public static Result EthernetConnect(Commor commor)
        {
            var socket = (Socket)commor.Connector;
            var ethernetCommor = (EthernetCommor)commor.Communicator;
            try
            {
                socket.Connect(ethernetCommor.IP, ethernetCommor.Port);
            }
            catch (Exception ex)
            {
                return new Result(ex.Message);
            }
            if (!socket.Connected)
            {
                return new Result(string.Format("连接失败：{0}：{1}", ethernetCommor.IP, ethernetCommor.Port));
            }
            return Result.OK;
        }

        public static Result SerialEndConnect(Commor commor)
        {
            var serialPort = (SerialPort)commor.Connector;
            try
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }
                serialPort.Dispose();
            }
            catch (Exception ex)
            {
                return new Result(ex.Message);
            }
            return Result.OK;
        }

        public static Result EthernetEndConnect(Commor commor)
        {
            var socket = (Socket)commor.Connector;
            if (socket != null)
            {
                socket.Close();
                socket = null;
            }
            return Result.OK;
        }

        public static Result EthernetComm(Commor commor, string input)
        {
            var socket = (Socket)commor.Connector;
            var ethernetCommor = (EthernetCommor)commor.Communicator;
            var recvData = string.Empty;
            try
            {

                if (!socket.Connected)
                {
                    socket = null;
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    commor.Connect();
                }
                else
                {
                    var readtimeout = 500;

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
                                getStr = Encoding.ASCII.GetString(Data).Trim('\0');
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
                            recvData = getStr.Replace("\r", "").Replace("\n", "").Trim('\0').Trim();
                        }
                        else
                        {
                            return new Result("读取数据超时：" + ethernetCommor.Name);
                        }
                    }
                    else
                    {
                        socket.Receive(Data);
                        recvData = Encoding.ASCII.GetString(Data).Trim('\0');
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = string.Format("和{0}通信出现异常！原因：{1}", ethernetCommor.Name, ex.Message);
                return new Result(msg);
            }
            return Result.OK;
        }

        public static Result SerialComm(Commor commor, string input)
        {
            var ethernetCommor = (EthernetCommor)commor.Communicator;
            var recvData = string.Empty;
            var serialPort = (SerialPort)commor.Connector;
            try
            {
                serialPort.Write(input);
                Thread.Sleep(500);
                Byte[] InputBuf = new Byte[128];
                serialPort.Read(InputBuf, 0, serialPort.BytesToRead);
                recvData = Encoding.ASCII.GetString(InputBuf).Trim('\0');

                if (string.IsNullOrEmpty(recvData))
                {
                    return new Result("规定时间内串口未返回数据:" + serialPort.PortName);
                }
            }
            catch (Exception ex)
            {
                return new Result(ex.Message);
            }
            return Result.OkHasData(recvData);
        }
    }
}
