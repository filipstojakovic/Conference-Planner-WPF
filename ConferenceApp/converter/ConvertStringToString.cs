using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using ConferenceApp.model.entity;
using Haley.Utils;

namespace ConferenceApp.converter
{
	public class ConvertStringToString : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
				return "";

			var text = (string)value;

			return LangUtils.Translate(text);
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
				return "";

			var text = (string)value;

			return LangUtils.Translate(text);
		}
	}
}
