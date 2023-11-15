using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk4s.Helpers.Converters
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is bool isScaledUp && isScaledUp)
		{
			return 1.0; // Normal scale
		}
		return 0.7; // Scaled down or any other value you prefer
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
}
