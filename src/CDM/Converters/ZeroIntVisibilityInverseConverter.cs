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
    public class ZeroIntVisibilityInverseConverter : IValueConverter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ZeroIntVisibilityInverseConverter()
        {

        }
        /// <summary>
        /// This method is convert inverse functionality
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
            return v <= 0 ? Visibility.Visible : Visibility.Collapsed;
        }
        /// <summary>
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
