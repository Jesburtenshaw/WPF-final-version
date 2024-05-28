using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CDM.Converters
{
    internal class ArrowPathConverter : IValueConverter
    {
        /// <summary>
        /// This method convert object type to Arrow icon
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isAscending = (bool)value;
            return isAscending ? "M0 0 L5 10 L10 0 Z" : "M0 10 L5 0 L10 10 Z";
        }
        /// <summary>
        /// This method convert Arrow icon to object type
        /// Yet not implemented.
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
