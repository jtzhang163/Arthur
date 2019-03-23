using Arthur.Core;
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
            this.DataContext = Current.App;
            this.commorList.DataContext = GMCC.Sorter.Factory.Commor.GetCommors();
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
                    MessageBox.Show("系统已经在运行，请勿重复启动！", "提示", MessageBoxButton.OK);
                    return;
                }

                if (Current.App.RunStatus == RunStatus.异常)
                {
                    MessageBox.Show("请先复位！", "提示", MessageBoxButton.OK);
                    return;
                }

                if (Running.Start().IsOk)
                {
                    MessageBox.Show("成功启动运行！", "提示", MessageBoxButton.OK);
                }

            }
            else if (btn.Name == "stop")
            {
                if (!TimerExec.IsRunning)
                {
                    MessageBox.Show("系统没有在运行，操作无效！", "提示", MessageBoxButton.OK);
                    return;
                }

                if (Running.Stop().IsOk)
                {
                    MessageBox.Show("成功停止运行！", "提示", MessageBoxButton.OK);
                }

            }
            else if (btn.Name == "reset")
            {

                if (Current.App.RunStatus == RunStatus.运行)
                {
                    MessageBox.Show("系统正在运行，复位无效，请停止运行后再执行复位操作！", "提示", MessageBoxButton.OK);
                    return;
                }

                if (Current.App.RunStatus == RunStatus.闲置)
                {
                    MessageBox.Show("系统尚未启动，复位操作无效！", "提示", MessageBoxButton.OK);
                    return;
                }

                if (Running.Reset().IsOk)
                {
                    MessageBox.Show("成功复位！", "提示", MessageBoxButton.OK);
                }

            }
        }

        private void TaskControl_Click(object sender, RoutedEventArgs e)
        {
            var btn = (ProcButton)sender;
            if(btn.Name == "task_mode_switch")
            {
                if (Current.App.RunStatus == RunStatus.闲置)
                {
                    MessageBox.Show("请先启动！", "提示", MessageBoxButton.OK);
                    return;
                }

                if (Current.App.RunStatus == RunStatus.运行)
                {
                    MessageBox.Show("请先停止！", "提示", MessageBoxButton.OK);
                    return;
                }

                if (!Current.App.IsTerminalInitFinished)
                {
                    MessageBox.Show("烤箱信息初始化尚未完成，请稍后！", "提示", MessageBoxButton.OK);
                    return;
                }

                //if (Current.App.TaskMode == TaskMode.手动任务 && Current.Task.Status != TaskStatus.完成)
                //{
                //    Tip.Alert("当前手动任务尚未完成，无法切换为自动任务！若要强制切换，请先点击任务复位。");
                //    return;
                //}


                //if (Current.App.TaskMode == TaskMode.手动任务)
                //{


                //}



                //if (Current.App.TaskMode == TaskMode.自动任务 && Current.Task.Status != TaskStatus.完成)
                //{

                //    Tip.Alert("当前任务完成后会切换至手动任务。若要立即切换，请点击任务复位");
                //    CurrentTask.ToSwitchManuTaskMode = true;
                //    return;
                //}

                Current.App.TaskMode = Current.App.TaskMode == TaskMode.自动任务 ? TaskMode.手动任务 : TaskMode.自动任务;
                MessageBox.Show(string.Format("成功切换为{0}！", Current.App.TaskMode), "提示", MessageBoxButton.OK);
            }
        }
    }
}
