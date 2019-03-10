using System;
using System.Collections.Generic;
using System.Linq;
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
using SzYitong.Bis.App.UserControls;

namespace SzYitong.Bis.App
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Title = new Utility().AppTitle;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radio = sender as RadioButton;
            if(radio.Content.ToString() == "个人中心")
            {
                var a = new TabItemClose();
               // a.Cursor = new Cursor("Hand");
                a.Header = "个人中心";
                a.Height = 30;
                a.Width = 100;

                var g = new Grid();

                var t = new CurrentUserInfoUC
                {
                };
                g.Children.Add(t);
                g.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#aaffffff"));

                a.Content = g;
                this.tabControl.Items.Add(a);
            }
        }
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
                if(itemclose.Header.ToString() == "主界面")
                {
                    return;
                }
                (itemclose.Parent as TabControl).Items.Remove(itemclose);
                RoutedEventArgs args = new RoutedEventArgs(TabItemClose.CloseItemEvent, itemclose);
                itemclose.RaiseEvent(args);
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
