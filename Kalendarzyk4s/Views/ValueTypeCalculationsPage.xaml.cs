using Kalendarzyk4s.Helpers;
using Kalendarzyk4s.Services.DataOperations;
using Kalendarzyk4s.ViewModels;
using Kalendarzyk4s.ViewModels.EventOperations;

namespace Kalendarzyk4s.Views;

public partial class ValueTypeCalculationsPage : ContentPage
{
	public ValueTypeCalculationsPage()
	{
		var viewModel = ServiceHelper.GetService<ValueTypeCalculationsViewModel>();

		BindingContext = viewModel;
		InitializeComponent();
	}
	protected override void OnAppearing()
	{
		base.OnAppearing();
		var viewModel = BindingContext as ValueTypeCalculationsViewModel;
		viewModel.OnAppearing();
	}
}