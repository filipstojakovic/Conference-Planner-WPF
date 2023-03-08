using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ConferenceApp.converter
{
	//convert imagePath to image. Used in DataGrid to show images in columns
	//maybe in future project
	public class ConvertTextToImage : IValueConverter
	{
		public object? Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
				return null;

			var path = new Uri(value.ToString(), UriKind.RelativeOrAbsolute);
			return new BitmapImage(path);
		}
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return "";
		}
	}
}