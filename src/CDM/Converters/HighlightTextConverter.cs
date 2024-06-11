using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace CDM.Converters
{
    public class HighlightTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null || values[1] == null)
                return values[0];

            string text = values[0].ToString();
            string searchText = values[1].ToString();

            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(searchText))
                return text;

            TextBlock textBlock = new TextBlock();
            int index = text.IndexOf(searchText, StringComparison.OrdinalIgnoreCase);
            if (index < 0)
            {
                textBlock.Text = text; // No match found, return original text
                return textBlock;
            }

            // Create a Run for the text before the match
            if (index > 0)
            {
                textBlock.Inlines.Add(new Run(text.Substring(0, index)));
            }

            // Create a Run for the match with highlighted background
            Run highlightRun = new Run(text.Substring(index, searchText.Length))
            {
                Background = Brushes.Yellow
            };
            textBlock.Inlines.Add(highlightRun);

            // Create a Run for the text after the match
            if (index + searchText.Length < text.Length)
            {
                textBlock.Inlines.Add(new Run(text.Substring(index + searchText.Length)));
            }

            return textBlock;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
