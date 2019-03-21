using GMCC.Sorter.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

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

}
