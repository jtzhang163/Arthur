using System;
using System.Collections.Generic;
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

namespace Arthur.Helper.TcpServer
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

        TcpListener server = null;
        TcpClient client = null;
        byte[] bytes = new byte[256];
        string data = null;
        NetworkStream stream;

        public void Run()
        {

            try
            {
                while (Current.App.IsRunning)
                {
                    ReceiveSendDataAdd("Waiting for a connection... ");
                    client = server.AcceptTcpClient();
                    ReceiveSendDataAdd("Connected!");
                    data = null;
                    stream = client.GetStream();

                    int i;
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i).Trim(new char[] { '\r', '\n' });
                        ReceiveSendDataAdd("Received: " + data.Trim(new char[] { '\r', '\n' }));
                        if (data == Current.App.Receive1)
                        {
                            byte[] send = System.Text.Encoding.ASCII.GetBytes(Current.App.Send1 + "\r");
                            stream.Write(send, 0, send.Length);
                            ReceiveSendDataAdd("Sent: " + Current.App.Send1);
                        }
                        else if (data == Current.App.Receive2)
                        {
                            byte[] send = System.Text.Encoding.ASCII.GetBytes(Current.App.Send2 + "\r");
                            stream.Write(send, 0, send.Length);
                            ReceiveSendDataAdd("Sent: " + Current.App.Send2);
                        }
                        else if (data == Current.App.Receive3)
                        {
                            byte[] send = System.Text.Encoding.ASCII.GetBytes(Current.App.Send3 + "\r");
                            stream.Write(send, 0, send.Length);
                            ReceiveSendDataAdd("Sent: " + Current.App.Send3);
                        }
                        else if (data == Current.App.Receive4)
                        {
                            byte[] send = System.Text.Encoding.ASCII.GetBytes(Current.App.Send4 + "\r");
                            stream.Write(send, 0, send.Length);
                            ReceiveSendDataAdd("Sent: " + Current.App.Send4);
                        }

                    }

                    client.Close();
                }
            }
            catch (Exception e)
            {
                ReceiveSendDataAdd("SocketException:" + e);
                ReceiveSendDataAdd("Hit enter to continue...");
            }
            finally
            {
                if (server != null)
                {
                    server.Stop();
                }
            }

        }

        public void ReceiveSendDataAdd(string info)
        {
            Current.App.Tip += info + "\r\n";
        }

        Thread t;

        private void btnListen_Click(object sender, RoutedEventArgs e)
        {

            Current.App.IsRunning = !Current.App.IsRunning;

            if (Current.App.IsRunning)
            {
                server = new TcpListener(IPAddress.Parse(Current.App.IP), Current.App.Port);
                server.Start();

                t = new Thread(() => {
                    Run();
                });
                t.Start();
            }
            else
            {
                server.Stop();
                server = null;
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
