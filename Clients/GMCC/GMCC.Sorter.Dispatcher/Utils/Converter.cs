using GMCC.Sorter.Utils;
using GMCC.Sorter.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace GMCC.Sorter.Dispatcher.Utils
{

    public class RightInfoConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var companyName = (string)value;
            var appInfo = new AppInfo();
            var devYear = appInfo.UpdateTime.Year;
            var time = devYear == DateTime.Now.Year ? DateTime.Now.Year.ToString() : appInfo.UpdateTime.Year + "-" + DateTime.Now.Year;
            return string.Format("© {0} {1}", time, companyName);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class VersionConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new AppInfo().VersionInfo.TrimStart('V');
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class UpdateTimeConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new AppInfo().UpdateTime.ToString("yyyy/M/d");
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class RunStatusBackgroundConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var runStatus = (RunStatus)value;
            return runStatus == RunStatus.运行 ? Brushes.Lime :
                runStatus == RunStatus.异常 ? Brushes.Red :
                runStatus == RunStatus.停止 ? Brushes.Yellow : Brushes.LightGray;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TaskModeSwitchBtnContentConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var taskMode = (TaskMode)value;
            return taskMode == TaskMode.自动任务 ? "切换手动":"切换自动";
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TaskModeSwitchBtnBackgroundConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var taskMode = (TaskMode)value;
            return taskMode == TaskMode.自动任务 ? Brushes.Lime : Brushes.LightGray;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MachineIsAliveToBackgroundConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isAlive = (bool)value;
            return isAlive ? Brushes.Lime : Brushes.LightCyan;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
