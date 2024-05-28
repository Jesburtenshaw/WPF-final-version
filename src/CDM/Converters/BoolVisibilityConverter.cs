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
    public class BoolVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BoolVisibilityConverter()
        {

        }
        /// <summary>
        /// This method convert bool type to visibility
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (null == value) return false;
            bool visible = (bool)value;
            return visible ? Visibility.Visible : Visibility.Collapsed;
        }
        /// <summary>
        /// This method convert visibility to bool
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
