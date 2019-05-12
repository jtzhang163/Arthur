using GMCC.Sorter.Data;
using GMCC.Sorter.Model;
using GMCC.Sorter.Utils;
using GMCC.Sorter.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

    public class ErrorMsgToVisibilityConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var errorMsg = (string)value;
            return string.IsNullOrEmpty(errorMsg) ? Visibility.Collapsed : Visibility.Visible;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class JawPosToMarginConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var pos = (int)value;
            return new Thickness(pos, 0, 0, 0); ;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SignalToSingalColorConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var signal = (bool)value;
            return signal ? Brushes.Lime : Brushes.LightGray;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IdToProcTrayCodeConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var id = (int)value;
            using (var db = new Data.AppContext())
            {
                return (db.ProcTrays.FirstOrDefault(o => o.Id == id) ?? new Model.ProcTray()).Code;
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IdToStorageNameConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var id = (int)value;
            return (Current.Storages.FirstOrDefault(o => o.Id == id) ?? new StorageViewModel()).Name;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ProcTrayIdToBackConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var id = (int)value;
            return id > 0 ? Brushes.LightGreen : Brushes.White;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class IsJawHasTrayToBackConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isJawHasTray = (bool)value;
            return isJawHasTray ? (Current.Task.Type == TaskType.上料 ? Brushes.LightGreen : Brushes.Cyan) : Brushes.White;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsJawHasTrayToProcTrayCodeConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isJawHasTray = (bool)value;
            if (!isJawHasTray)
            {
                return "";
            }
            else
            {
                using (var db = new Data.AppContext())
                {
                    return (db.ProcTrays.FirstOrDefault(o => o.Id == Current.Task.ProcTrayId) ?? new Model.ProcTray()).Code;
                }
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ProcTrayIdToVisibilityConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var id = (int)value;
            return id > 0 ? Visibility.Visible : Visibility.Hidden;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class StorageBackgroundConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var status = StorageStatus.未知;
            var isEnabled = false;
            var taskType = Model.TaskType.未知;
            var timeNow = Arthur.Default.DateTime;

            if (parameter as string == "StatusAndIsEnabled")
            {
                status = (StorageStatus)values[0];
                isEnabled = (bool)values[1];
                taskType = (Model.TaskType)values[2];
                timeNow = (DateTime)values[3];
            }

            if (!isEnabled)
            {
                return Brushes.LightGray;
            }

            if (taskType != TaskType.未知 && timeNow.Second % 2 == 1)
            {
                if (taskType == TaskType.上料)
                {
                    return Brushes.LightGreen;
                }
                else
                {
                    return Brushes.White;
                }
            }

            return status == StorageStatus.无托盘 ? Brushes.White :
                status == StorageStatus.正在静置 ? Brushes.LightGreen :
                status == StorageStatus.静置完成 ? Brushes.Cyan :
                Brushes.White;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }


    public class TaskTypeToVisibilityConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var taskType = (TaskType)value;
            return taskType == TaskType.未知 ? Visibility.Hidden : Visibility.Visible;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class TaskStatusToForegroundConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var taskStatus = (Model.TaskStatus)value;
            return taskStatus == Model.TaskStatus.未知 || taskStatus == Model.TaskStatus.完成 ? Brushes.Green : Brushes.White;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class TaskStatusToBackgroundConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var taskStatus = (Model.TaskStatus)value;
            return taskStatus == Model.TaskStatus.未知 || taskStatus == Model.TaskStatus.完成 ? Brushes.Transparent : Brushes.Green;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class HasTrayToForegroundConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Brushes.LimeGreen : Brushes.Black;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
