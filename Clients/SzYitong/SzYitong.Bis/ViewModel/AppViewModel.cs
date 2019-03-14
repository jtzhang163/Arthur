using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SzYitong.Bis.Utils;

namespace SzYitong.Bis.ViewModel
{
    public class AppViewModel : Arthur.ViewModel.AppViewModel
    {

        private string appName = null;

        public string AppName
        {
            get
            {
                if (appName == null)
                {
                    appName = Current.Option.AppName;
                }
                return appName;
            }
            set
            {
                if (appName != value)
                {
                    Current.Option.AppName = value;

                    //更新AppName时同步更新AppTitle，使窗体标题栏变化
                    AppTitle = AppNameToTitle(value);

                    SetProperty(ref appName, value);
                }
            }
        }

        private string appTitle = null;

        public string AppTitle
        {
            get
            {
                if (appTitle == null)
                {
                    appTitle = AppNameToTitle(AppName);
                }
                return appTitle;
            }
            set
            {
                if (appTitle != value)
                {
                    SetProperty(ref appTitle, value);
                }
            }
        }

        private string AppNameToTitle(string appName)
        {
            var appInfo = new AppInfo();
            return string.Format("{0} {1}   {2}", appName, appInfo.VersionInfo, appInfo.UpdateTime.ToString("yyyy/M/d"));
        }
    }
}
