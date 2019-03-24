using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Arthur.Helper.ComServer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = Current.App;
        }

        SerialPort serialPort = null;

        public void Run()
        {

            try
            {
                while (Current.App.IsRunning)
                {
                    if (serialPort.BytesToRead > 0)
                    {
                        var receiveString = string.Empty;
                        do
                        {
                            int count = serialPort.BytesToRead;
                            if (count <= 0)
                                break;
                            byte[] readBuffer = new byte[count];
                            serialPort.Read(readBuffer, 0, count);
                            receiveString += System.Text.Encoding.Default.GetString(readBuffer).Trim('\0').Trim('\r').Trim('\n');
                            Thread.Sleep(10);
                        } while (serialPort.BytesToRead > 0);

                        //Byte[] InputBuf = new Byte[128];
                        //serialPort.Read(InputBuf, 0, serialPort.BytesToRead);
                        //receiveString = Encoding.ASCII.GetString(InputBuf).Trim('\0');

                        ReceiveSendDataAdd("Received: " + receiveString.Trim(new char[] { '\r', '\n' }));
                        if (receiveString.Contains(Current.App.Receive1))
                        {
                            serialPort.Write(Current.App.Send1);
                            ReceiveSendDataAdd("Sent: " + Current.App.Send1);
                        }
                        else if (receiveString.Contains(Current.App.Receive2))
                        {
                            serialPort.Write(Current.App.Send2);
                            ReceiveSendDataAdd("Sent: " + Current.App.Send2);
                        }
                        else if (receiveString.Contains(Current.App.Receive3))
                        {
                            serialPort.Write(Current.App.Send3);
                            ReceiveSendDataAdd("Sent: " + Current.App.Send3);
                        }
                        else if (receiveString.Contains(Current.App.Receive4))
                        {
                            serialPort.Write(Current.App.Send4);
                            ReceiveSendDataAdd("Sent: " + Current.App.Send4);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ReceiveSendDataAdd("com Exception:" + e);
            }
            finally
            {

            }

        }

        public void ReceiveSendDataAdd(string info)
        {
            Current.App.Tip += info + "\r\n";
        }

        private void btnOpenOrClose_Click(object sender, RoutedEventArgs e)
        {

            Current.App.IsRunning = !Current.App.IsRunning;

            if (Current.App.IsRunning)
            {
                serialPort = new SerialPort((string)selectedCom.SelectedItem);
                serialPort.Open();

                var t = new Thread(() => {
                    Run();
                });
                t.Start();
            }
            else
            {
                serialPort.Close();
                serialPort = null;
            }
        }

        private void btnClearShow_Click(object sender, RoutedEventArgs e)
        {
            Current.App.Tip = "";
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
            System.Environment.Exit(System.Environment.ExitCode);
        }
    }
}
