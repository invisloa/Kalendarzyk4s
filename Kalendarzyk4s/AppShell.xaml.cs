using Kalendarzyk4s.Views;
using System.Globalization;

namespace Kalendarzyk4s;

public partial class AppShell : Shell
{
	public AppShell()
	{

		// REGISTER ROUTING
		Routing.RegisterRoute(nameof(AddNewSubTypePage), typeof(AddNewSubTypePage));
		Routing.RegisterRoute(nameof(AllSubTypesPage), typeof(AllSubTypesPage));



		InitializeComponent();
	}
}
