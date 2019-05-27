using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Threading;
using Arthur.App.View.Utils;
using GMCC.Sorter.Dispatcher.Views;
using GMCC.Sorter.Dispatcher.Views.SystemUC;
using GMCC.Sorter.Run;

namespace GMCC.Sorter.Dispatcher
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (SystemParameters.PrimaryScreenHeight < 800)
            {
                this.WindowState = WindowState.Maximized;
            }

            grid_mainview.Children.Add(new MainViewUC());

            this.DataContext = Current.App;
            Arthur.App.Business.Logging.AddEvent(string.Format("打开软件", ""), Arthur.App.Model.EventType.信息);

            WinSet.MainWindow = this;

            objs = ControlsSearchHelper.GetChildObjects<RadioButton>(this.nav_bar, "");
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

            RadioButton radio = sender as RadioButton;
            SetOtherNavNoChecked(radio);
            var tabName = radio.Content.ToString();


            var isContain = false;
            foreach (TabItem tabItem in this.tabControl.Items)
            {
                if (tabItem.Header.ToString() == tabName) { isContain = true; }
            }

            if (!isContain)
            {

                if ((this.tabControl.Items.Count + 1) * 100 > this.ActualWidth - 210) /*this.tabControl.Width为NaN*/
                {
                    MessageBox.Show("打开的选项卡过多！");
                    this.SetNavNoChecked(tabName);
                    this.SetOneNavChecked();
                    return;
                }

                var a = new TabItemClose();
                //a.Cursor = Cursors.Hand;
                a.Header = tabName;
                a.Height = 30;
                a.Width = 100;

                var g = new Grid();
                g.Children.Add(GetTabItem(tabName));

                //g.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFECF0F5"));
                ImageBrush ib = new ImageBrush() {ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/Character.jpg", UriKind.RelativeOrAbsolute)), Opacity = 0.1 };
                g.Background = ib;

                a.Content = g;
                //this.tabControl.Items.Add(a);
                IsChangeNavChecked = true;
                this.tabControl.SelectedIndex = this.tabControl.Items.Add(a);
                IsChangeNavChecked = false;
            }
            else
            {
                for (var i = 0; i < this.tabControl.Items.Count; i++)
                {
                    if ((this.tabControl.Items[i] as TabItem).Header.ToString() == tabName)
                    {
                        IsChangeNavChecked = true;
                        this.tabControl.SelectedIndex = i;
                        IsChangeNavChecked = false;
                        return;
                    }
                }
            }

            // IsChangeNavChecked = false;
        }

        private UIElement GetTabItem(string tabName)
        {
            switch (tabName)
            {
                case "个人中心":
                    return new CurrentUserUC();
                case "用户管理":
                    return new UserManageUC();
                case "角色管理":
                    return new RoleManageUC();
                case "系统参数":
                    return new ParamUC();
                case "系统事件":
                    return new EventLogUC();
                case "系统日志":
                    return new OplogUC();

                case "配置":
                    return new OptionView();


                case "PLC":
                    return new MachinePlcView();
                case "电池扫码枪":
                    return new BatteryScanerView();
                case "托盘扫码枪":
                    return new TrayScanerView();
                case "托盘管理":
                    return new TrayView();
                case "料仓管理":
                    return new StorageView();

                case "PLC调试":
                    return new PlcView();
                case "扫码枪调试":
                    return new ScanerView();

                case "产能与合格率":
                    return new ProductivityView();

                case "托盘追溯":
                    return new ProcTrayView();
                case "电池追溯":
                    return new BatteryView();
                case "任务日志":
                    return new TaskLogView();

                case "BTS客户端":
                    return new BtsClientView();
                case "MES":
                    return new MESView();

                case "二维码生成器":
                    return new QRCoderView();

                case "关于":
                    return new AboutView();
                default:
                    return null;
            }
        }

        private void currentUser_Click(object sender, RoutedEventArgs e)
        {
            var radio = new RadioButton() { Content = "个人中心" };
            RadioButton_Checked(radio, null);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (TimerExec.IsRunning)
            {
                MessageBox.Show("流程正在运行，请先停止！");
                e.Cancel = true;
                return;
            }

            var result = MessageBox.Show("确定要关闭当前程序？", "关闭确认", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
            else
            {
                GMCC.Sorter.Run.Running.Reset();
                Arthur.App.Business.Logging.AddEvent(string.Format("关闭软件", ""), Arthur.App.Model.EventType.信息);
            }
        }

        /// <summary>
        /// 其他RadioButton去掉选中
        /// </summary>
        private void SetOtherNavNoChecked(RadioButton radio)
        {
            var content = radio.Content.ToString();
            var objs = ControlsSearchHelper.GetChildObjects<RadioButton>(this.nav_bar, "");
            objs.ForEach(o =>
            {
                if (o.Content.ToString() != content && o.IsChecked.Value)
                {
                    o.IsChecked = false;
                }
            });
        }
        /// <summary>
        /// 关闭选项卡时导航栏栏目取消选中
        /// </summary>
        /// <param name="nav_name">栏目名称</param>
        public void SetNavNoChecked(string nav_name)
        {
            var objs = ControlsSearchHelper.GetChildObjects<RadioButton>(this.nav_bar, "");
            objs.ForEach(o =>
            {
                if (o.Content.ToString() == nav_name)
                {
                    o.IsChecked = false;
                }
            });
        }

        public void SetOneNavChecked()
        {
            var objs = ControlsSearchHelper.GetChildObjects<RadioButton>(this.nav_bar, "");
            objs.ForEach(o =>
            {
                if (o.Content.ToString() == (this.tabControl.Items[this.tabControl.SelectedIndex] as TabItem).Header.ToString())
                {
                    o.IsChecked = true;
                    return;
                }
            });
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsChangeNavChecked)
            {
                new Thread(() =>
                {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    new Action(() =>
                    {
                        Thread.Sleep(100);
                        tabControl_SelectionChanged(sender, e);
                    }));
                }).Start();
                return;
            }
            var selectedTab = this.tabControl.SelectedItem as TabItemClose;
            if (selectedTab == null)
            {
                return;
            }

            if (selectedTab.Header.ToString() == "主界面")
            {
                objs.ForEach(o =>
                {
                    o.IsChecked = false;
                });
                return;
            }

            objs = ControlsSearchHelper.GetChildObjects<RadioButton>(this.nav_bar, "");
            objs.ForEach(o =>
            {
                if (o.Content.ToString() == selectedTab.Header.ToString())
                {
                    o.IsChecked = true;
                }
            });
        }

        private bool IsChangeNavChecked = false;

        private List<RadioButton> objs;
    }


    public static class WinSet
    {
        public static MainWindow MainWindow;
    }


    public class TabItemClose : TabItem
    {
        static TabItemClose()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TabItemClose), new FrameworkPropertyMetadata(typeof(TabItemClose)));
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetValue(e.Property, e.NewValue);
        }

        /// <summary>
        /// 是否可以关闭
        /// </summary>
        public bool IsCanClose
        {
            get { return (bool)GetValue(IsCanCloseProperty); }
            set { SetValue(IsCanCloseProperty, value); }
        }

        public static readonly DependencyProperty IsCanCloseProperty =
            DependencyProperty.Register("IsCanClose", typeof(bool), typeof(TabItemClose), new PropertyMetadata(true, OnPropertyChanged));

        /// <summary>
        /// 关闭的图标
        /// </summary>
        public ImageSource CloseIcon
        {
            get { return (ImageSource)GetValue(CloseIconProperty); }
            set { SetValue(CloseIconProperty, value); }
        }

        public static readonly DependencyProperty CloseIconProperty =
            DependencyProperty.Register("CloseIcon", typeof(ImageSource), typeof(TabItemClose), new PropertyMetadata(null, OnPropertyChanged));



        /// <summary>
        /// 正常背景色
        /// </summary>
        public SolidColorBrush NormalBackground
        {
            get { return (SolidColorBrush)GetValue(NormalBackgroundProperty); }
            set { SetValue(NormalBackgroundProperty, value); }
        }

        public static readonly DependencyProperty NormalBackgroundProperty =
            DependencyProperty.Register("NormalBackground", typeof(SolidColorBrush), typeof(TabItemClose), new PropertyMetadata(null, OnPropertyChanged));

        /// <summary>
        /// 悬浮背景色
        /// </summary>
        public SolidColorBrush OverBackgound
        {
            get { return (SolidColorBrush)GetValue(OverBackgoundProperty); }
            set { SetValue(OverBackgoundProperty, value); }
        }

        public static readonly DependencyProperty OverBackgoundProperty =
            DependencyProperty.Register("OverBackgound", typeof(SolidColorBrush), typeof(TabItemClose), new PropertyMetadata(null, OnPropertyChanged));


        /// <summary>
        /// 选中背景色
        /// </summary>
        public SolidColorBrush SelectedBackgound
        {
            get { return (SolidColorBrush)GetValue(SelectedBackgoundProperty); }
            set { SetValue(SelectedBackgoundProperty, value); }
        }

        public static readonly DependencyProperty SelectedBackgoundProperty =
            DependencyProperty.Register("SelectedBackgound", typeof(SolidColorBrush), typeof(TabItemClose), new PropertyMetadata(null, OnPropertyChanged));


        /// <summary>
        /// 默认前景色
        /// </summary>
        public SolidColorBrush NormalForeground
        {
            get { return (SolidColorBrush)GetValue(NormalForegroundProperty); }
            set { SetValue(NormalForegroundProperty, value); }
        }

        public static readonly DependencyProperty NormalForegroundProperty =
            DependencyProperty.Register("NormalForeground", typeof(SolidColorBrush), typeof(TabItemClose), new PropertyMetadata(null, OnPropertyChanged));

        /// <summary>
        /// 悬浮前景色
        /// </summary>
        public SolidColorBrush OverForeground
        {
            get { return (SolidColorBrush)GetValue(OverForegroundProperty); }
            set { SetValue(OverForegroundProperty, value); }
        }

        public static readonly DependencyProperty OverForegroundProperty =
            DependencyProperty.Register("OverForeground", typeof(SolidColorBrush), typeof(TabItemClose), new PropertyMetadata(null, OnPropertyChanged));

        /// <summary>
        /// 选中前景色
        /// </summary>
        public SolidColorBrush SelectedForeground
        {
            get { return (SolidColorBrush)GetValue(SelectedForegroundProperty); }
            set { SetValue(SelectedForegroundProperty, value); }
        }

        public static readonly DependencyProperty SelectedForegroundProperty =
            DependencyProperty.Register("SelectedForeground", typeof(SolidColorBrush), typeof(TabItemClose), new PropertyMetadata(null, OnPropertyChanged));


        /// <summary>
        /// 控件圆角
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(TabItemClose), new PropertyMetadata(new CornerRadius(0), OnPropertyChanged));


        /// <summary>
        /// 前置Logo
        /// </summary>
        public ImageSource LogoIcon
        {
            get { return (ImageSource)GetValue(LogoIconProperty); }
            set { SetValue(LogoIconProperty, value); }
        }

        public static readonly DependencyProperty LogoIconProperty =
            DependencyProperty.Register("LogoIcon", typeof(ImageSource), typeof(TabItemClose), new PropertyMetadata(null, OnPropertyChanged));



        /// <summary>
        /// 前置Logo宽度
        /// </summary>
        public double LogoIconWidth
        {
            get { return (double)GetValue(LogoIconWidthProperty); }
            set { SetValue(LogoIconWidthProperty, value); }
        }

        public static readonly DependencyProperty LogoIconWidthProperty =
            DependencyProperty.Register("LogoIconWidth", typeof(double), typeof(TabItemClose), new PropertyMetadata(double.Parse("0"), OnPropertyChanged));


        /// <summary>
        /// 前置Logo高度
        /// </summary>
        public double LogoIconHeigth
        {
            get { return (double)GetValue(LogoIconHeigthProperty); }
            set { SetValue(LogoIconHeigthProperty, value); }
        }

        public static readonly DependencyProperty LogoIconHeigthProperty =
            DependencyProperty.Register("LogoIconHeigth", typeof(double), typeof(TabItemClose), new PropertyMetadata(double.Parse("0"), OnPropertyChanged));


        /// <summary>
        /// LogoPadding
        /// </summary>
        public Thickness LogoPadding
        {
            get { return (Thickness)GetValue(LogoPaddingProperty); }
            set { SetValue(LogoPaddingProperty, value); }
        }

        public static readonly DependencyProperty LogoPaddingProperty =
            DependencyProperty.Register("LogoPadding", typeof(Thickness), typeof(TabItemClose), new PropertyMetadata(new Thickness(0), OnPropertyChanged));

        /// <summary>
        /// 关闭item事件
        /// </summary>
        public event RoutedEventHandler CloseItem
        {
            add { AddHandler(CloseItemEvent, value); }
            remove { RemoveHandler(CloseItemEvent, value); }
        }
        public static readonly RoutedEvent CloseItemEvent =
            EventManager.RegisterRoutedEvent("CloseItem", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TabItemClose));

        /// <summary>
        /// 关闭项的右键菜单
        /// </summary>
        public ContextMenu ItemContextMenu { get; set; }

        Border ItemBorder;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ItemBorder = Template.FindName("_bordertop", this) as Border;
            if (ItemContextMenu != null)
            {
                ItemBorder.ContextMenu = ItemContextMenu;
            }
        }
    }

    public class ButtonEx : Button
    {
        static ButtonEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ButtonEx), new FrameworkPropertyMetadata(typeof(ButtonEx)));
        }


        public ButtonType ButtonType
        {
            get { return (ButtonType)GetValue(ButtonTypeProperty); }
            set { SetValue(ButtonTypeProperty, value); }
        }

        public static readonly DependencyProperty ButtonTypeProperty =
            DependencyProperty.Register("ButtonType", typeof(ButtonType), typeof(ButtonEx), new PropertyMetadata(ButtonType.Normal));


        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ImageSource), typeof(ButtonEx), new PropertyMetadata(null));


        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ButtonEx), new PropertyMetadata(new CornerRadius(0)));


        public Brush MouseOverForeground
        {
            get { return (Brush)GetValue(MouseOverForegroundProperty); }
            set { SetValue(MouseOverForegroundProperty, value); }
        }

        public static readonly DependencyProperty MouseOverForegroundProperty =
            DependencyProperty.Register("MouseOverForeground", typeof(Brush), typeof(ButtonEx), new PropertyMetadata());


        public Brush MousePressedForeground
        {
            get { return (Brush)GetValue(MousePressedForegroundProperty); }
            set { SetValue(MousePressedForegroundProperty, value); }
        }

        public static readonly DependencyProperty MousePressedForegroundProperty =
            DependencyProperty.Register("MousePressedForeground", typeof(Brush), typeof(ButtonEx), new PropertyMetadata());


        public Brush MouseOverBorderbrush
        {
            get { return (Brush)GetValue(MouseOverBorderbrushProperty); }
            set { SetValue(MouseOverBorderbrushProperty, value); }
        }

        public static readonly DependencyProperty MouseOverBorderbrushProperty =
            DependencyProperty.Register("MouseOverBorderbrush", typeof(Brush), typeof(ButtonEx), new PropertyMetadata());


        public Brush MouseOverBackground
        {
            get { return (Brush)GetValue(MouseOverBackgroundProperty); }
            set { SetValue(MouseOverBackgroundProperty, value); }
        }

        public static readonly DependencyProperty MouseOverBackgroundProperty =
            DependencyProperty.Register("MouseOverBackground", typeof(Brush), typeof(ButtonEx), new PropertyMetadata());


        public Brush MousePressedBackground
        {
            get { return (Brush)GetValue(MousePressedBackgroundProperty); }
            set { SetValue(MousePressedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty MousePressedBackgroundProperty =
            DependencyProperty.Register("MousePressedBackground", typeof(Brush), typeof(ButtonEx), new PropertyMetadata());

        protected override void OnClick()
        {
            base.OnClick();

            if (!string.IsNullOrEmpty(Name) && Name == "PART_Close_TabItem")
            {
                TabItemClose itemclose = FindVisualParent<TabItemClose>(this);

                (itemclose.Parent as TabControl).Items.Remove(itemclose);
                RoutedEventArgs args = new RoutedEventArgs(TabItemClose.CloseItemEvent, itemclose);
                itemclose.RaiseEvent(args);

                WinSet.MainWindow.SetNavNoChecked(itemclose.Header.ToString());
            }

        }

        public static T FindVisualParent<T>(DependencyObject obj) where T : class
        {
            while (obj != null)
            {
                if (obj is T)
                    return obj as T;

                obj = VisualTreeHelper.GetParent(obj);
            }

            return null;
        }
    }

    public enum ButtonType
    {
        Normal,
        Icon,
        Text,
        IconText
    }
}
