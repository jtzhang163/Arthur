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

namespace Arthur.Controls
{
    /// <summary>
    /// ProcButton.xaml 的交互逻辑
    /// </summary>
    public partial class ProcButton : Button
    {
        public ProcButton()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ForegroundCoreProperty = DependencyProperty.Register("ForegroundCore", typeof(Brush), typeof(ProcButton), new PropertyMetadata(ChangeForeground));
        public static readonly DependencyProperty AwesomeTextProperty = DependencyProperty.Register("AwesomeText", typeof(string), typeof(ProcButton), new PropertyMetadata(ChangeAwesomeText));
        public static readonly DependencyProperty ContentCoreProperty = DependencyProperty.Register("ContentCore", typeof(string), typeof(ProcButton), new PropertyMetadata(ChangeContentCore));


           
        public Brush ForegroundCore
        {
            get { return (Brush)GetValue(ForegroundCoreProperty); }
            set { SetValue(ForegroundCoreProperty, value); }
        }

        public string AwesomeText
        {
            get { return (string)GetValue(AwesomeTextProperty); }
            set { SetValue(AwesomeTextProperty, value); }
        }


        public string ContentCore
        {
            get { return (string)GetValue(ContentCoreProperty); }
            set { SetValue(ContentCoreProperty, value); }
        }


        private static void ChangeForeground(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var procButton = (ProcButton)d;
            procButton.AwesomeContent.Foreground = (Brush)e.NewValue;
            procButton.ProcContent.Foreground = (Brush)e.NewValue;
        }

        private static void ChangeAwesomeText(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var procButton = (ProcButton)d;
            procButton.AwesomeContent.Text = (string)e.NewValue;
        }

        private static void ChangeContentCore(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var procButton = (ProcButton)d;
            procButton.ProcContent.Text = (string)e.NewValue;
        }
    }
}
