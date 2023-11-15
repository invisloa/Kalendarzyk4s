﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk4s.Helpers.Converters
{
	public class BoolToAlphaConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool isOpaque && isOpaque)
			{
				return 1.0; // Fully opaque
			}
			return 0.45; // Semi-transparent or any other value you prefer
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
