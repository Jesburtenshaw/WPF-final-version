using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace CDM.Converters
{
    public class ZeroIntVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ZeroIntVisibilityConverter()
        {

        }
        /// <summary>
        /// This method is convert zero to visibility
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (null == value) return Visibility.Collapsed;
            int v = (int)value;
            return v > 0 ? Visibility.Visible : Visibility.Collapsed;
        }
        /// <summary>
        /// This method is used to convert back
        /// Yet not implemented
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
