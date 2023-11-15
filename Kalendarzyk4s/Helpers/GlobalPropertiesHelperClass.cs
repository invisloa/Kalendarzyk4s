﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk4s.Helpers
{
	public static class GlobalPropertiesHelperClass
	{
		public static void SetPrimaryColor(Color newColor)
		{
			if (Application.Current.Resources.ContainsKey("PrimaryColor"))
			{
				Application.Current.Resources["PrimaryColor"] = newColor;
			}
		}
	}
}
