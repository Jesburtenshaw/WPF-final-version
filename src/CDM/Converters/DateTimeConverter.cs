using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CDM.Converters
{
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {


            if (value != null && value is DateTime valueDate)
            {
                DateTime now = DateTime.Now;
                TimeSpan timeSpan = now - valueDate;

                string timeText = string.Empty;

                if (timeSpan.TotalSeconds < 300)
                {
                    timeText = "Just Now";
                }
                else if (now.Date == valueDate.Date)
                {
                    timeText = "Today " + valueDate.ToString("HH:mm");
                }
                else if (now.Date - valueDate.Date == TimeSpan.FromDays(1))
                {
                    timeText = "Yesterday " + valueDate.ToString("HH:mm");
                }
                else if (now - valueDate < TimeSpan.FromDays(7))
                {
                    timeText = valueDate.ToString("dddd HH:mm");
                }
                else
                {
                    timeText = valueDate.ToString("M/d/yyyy hh:mm");
                }

                return timeText;
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //throw new NotImplementedException();
            return true;
        }
    }
}
