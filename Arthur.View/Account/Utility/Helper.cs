using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Arthur.View.Account
{
    public class Helper
    {
        public static void ExecuteParentWindowMethod(UserControl uc, string method_name)
        {
            Window parentWindow = Window.GetWindow(uc);
            Type type = parentWindow.GetType();
            MethodInfo mi = type.GetMethod(method_name);
            mi.Invoke(parentWindow, new object[] { });
        }
    }
}
