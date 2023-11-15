using Kalendarzyk4s.ViewModels;

namespace Kalendarzyk4s.Views;

public partial class PreferencesPage : ContentPage
{
	public PreferencesPage()
	{
		var vm = new PreferencesViewModel();
		BindingContext = vm;
		InitializeComponent();
	}
}