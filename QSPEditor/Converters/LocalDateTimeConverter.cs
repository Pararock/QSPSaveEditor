using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization.DateTimeFormatting;
using Windows.System.UserProfile;
using Windows.UI.Xaml.Data;

namespace QSPEditor.Converters
{
    public sealed class LocalDateTimeConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                if (value is DateTime dateTime)
                {
                    if (dateTime == DateTime.MinValue)
                    {
                        return "";
                    }
                    value = new DateTimeOffset(dateTime);
                }
                if (value is DateTimeOffset dateTimeOffset)
                {
                    string format = parameter as String ?? "shortdate";
                    var userLanguages = GlobalizationPreferences.Languages;

                    var dateFormatter = new DateTimeFormatter(format, userLanguages);
                    return dateFormatter.Format(dateTimeOffset.ToLocalTime());
                }
                return "N/A";
            }
            catch
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
