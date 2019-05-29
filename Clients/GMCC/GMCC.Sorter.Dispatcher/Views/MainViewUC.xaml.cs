using Arthur.App.Business;
using Arthur.Controls;
using GMCC.Sorter.Run;
using GMCC.Sorter.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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

namespace GMCC.Sorter.Dispatcher.Views
{
    /// <summary>
    /// MainViewUC.xaml 的交互逻辑
    /// </summary>
    public partial class MainViewUC : UserControl
    {

        public string Option { get; set; }

        public MainViewUC() : this("Details")
        {
            if (SystemParameters.PrimaryScreenHeight < 800)
            {
                this.grid_op.Margin = new Thickness(0);
            }

            this.DataContext = Current.App;
            this.commorList.DataContext = GMCC.Sorter.Factory.CommorFactory.GetCommors();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="option">显示选项 Index, Create, Details ,Edit</param>
        public MainViewUC(string option)
        {
            this.Option = option;
            InitializeComponent();
            SwitchWindow(option, 0);
        }


        public void SwitchWindow(string option, int id)
        {
            this.Option = option;
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type type = assembly.GetType("GMCC.Sorter.Dispatcher.UserControls.MainView." + this.Option);
            object page = Activator.CreateInstance(type, new object[] { id });
            grid.Children.Clear();
            this.grid.Children.Add((UIElement)page);
        }

        private void RunningControl_Click(object sender, RoutedEventArgs e)
        {
            var btn = (ProcButton)sender;
            if (btn.Name == "start")
            {
                if (TimerExec.IsRunning)
                {
                    Current.App.ErrorMsg = "系统已经在运行，请勿重复启动！";
                    return;
                }

                if (Current.App.RunStatus == RunStatus.异常)
                {
                    Current.App.ErrorMsg = "请先复位！";
                    return;
                }

                var result = Running.Start();
                if (result.IsSucceed)
                {
                    Logging.AddEvent("点击【启动】", Arthur.App.Model.EventType.信息);
                }
                else
                {
                    Running.ShowErrorMsg(result.Msg);
                }

            }
            else if (btn.Name == "stop")
            {

                if (!TimerExec.IsRunning)
                {
                    Current.App.ErrorMsg = "系统没有在运行，操作无效！";
                    return;
                }

                var result = Running.Stop();
                if (result.IsSucceed)
                {
                    Logging.AddEvent("点击【停止】", Arthur.App.Model.EventType.信息);
                }
                else
                {
                    Running.ShowErrorMsg(result.Msg);
                }

            }
            else if (btn.Name == "reset")
            {

                if (Current.App.RunStatus == RunStatus.运行)
                {
                    Current.App.ErrorMsg = "系统正在运行，复位无效，请停止运行后再执行复位操作！";
                    return;
                }

                var result = Running.Reset();
                if (result.IsSucceed)
                {
                    Logging.AddEvent("点击【复位】", Arthur.App.Model.EventType.信息);
                }
                else
                {
                    Running.ShowErrorMsg(result.Msg);
                }
            }
        }

        private void TaskControl_Click(object sender, RoutedEventArgs e)
        {
            var btn = (ProcButton)sender;
            if (btn.Name == "task_mode_switch")
            {
                if (!Current.App.IsTerminalInitFinished)
                {
                    Current.App.ErrorMsg = "信息初始化尚未完成，请稍后！";
                    return;
                }

                Current.App.TaskMode = Current.App.TaskMode == TaskMode.自动任务 ? TaskMode.手动任务 : TaskMode.自动任务;
                MessageBox.Show(string.Format("成功切换为{0}！", Current.App.TaskMode), "提示", MessageBoxButton.OK);
                Logging.AddEvent(string.Format("点击【切换{0}】", Current.App.TaskMode), Arthur.App.Model.EventType.信息);
            }
            else if (btn.Name == "task_reset")
            {
                Current.Task.Status = Model.TaskStatus.完成;
                Logging.AddEvent("点击【任务复位】", Arthur.App.Model.EventType.信息);
            }
        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Current.App.ErrorMsg = "";
        }
    }
}
