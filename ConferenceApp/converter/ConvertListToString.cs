using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using ConferenceApp.model.entity;

namespace ConferenceApp.converter
{
	public class ConvertListToString : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
				return "";

			BindingList<Role> roles = (BindingList<Role>)value;

			List<string> roleNames = roles.Select(role => role.Name).ToList();
			return string.Join(", ", roleNames);
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return "";
		}
	}
}
