using System;
using System.Globalization;
using System.Windows.Data;

namespace ConferenceApp.converter;

public class DateFormat : IValueConverter
{
    private const string DateFormatString = "dd.MM.yyyy";

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is DateTime date)
        {
            return date.ToString(DateFormatString);
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is string dateString)
        {
            if (DateTime.TryParseExact(dateString, DateFormatString, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                return date;
            }
        }

        return value;
    }
}