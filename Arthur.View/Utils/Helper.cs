using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Arthur.View.Utils
{
    public class Helper
    {
        /// <summary>
        /// 执行所在窗口的方法
        /// </summary>
        /// <param name="uc"></param>
        /// <param name="method_name"></param>
        public static void ExecuteParentWindowMethod(UserControl uc, string method_name)
        {
            Window parentWindow = Window.GetWindow(uc);
            Type type = parentWindow.GetType();
            MethodInfo mi = type.GetMethod(method_name);
            mi.Invoke(parentWindow, new object[] { });
        }

        /// <summary>
        /// 执行父级用户控件方法
        /// </summary>
        /// <param name="uc"></param>
        /// <param name="method_name"></param>
        public static void ExecuteParentUserControlMethod(UserControl uc, string parentUserControlName,string methodName, string pageName)
        {
            UserControl parentUc = ControlsSearchHelper.GetParentObject<UserControl>(uc, parentUserControlName);
            Type type = parentUc.GetType();
            MethodInfo mi = type.GetMethod(methodName);
            mi.Invoke(parentUc, new object[] { pageName });
        }
    }
}
