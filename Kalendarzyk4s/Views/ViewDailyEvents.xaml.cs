using Kalendarzyk4s.Helpers;
using Kalendarzyk4s.Models.EventTypesModels;
using Kalendarzyk4s.Services.DataOperations;
using Kalendarzyk4s.ViewModels.EventsViewModels;

namespace Kalendarzyk4s.Views;

public partial class ViewDailyEvents : ContentPage
{
	public ViewDailyEvents()
	{
		var viewModel = ServiceHelper.GetService<DailyEventsViewModel>();
		BindingContext = viewModel;
		InitializeComponent();
		viewModel.OnEventsToShowListUpdated += () =>
		{
			viewModel.BindDataToScheduleList();
		};
	}
	protected override void OnDisappearing()
	{
		base.OnDisappearing();
		(BindingContext as DailyEventsViewModel).OnEventsToShowListUpdated -= (BindingContext as DailyEventsViewModel).BindDataToScheduleList;
	}
	public ViewDailyEvents(IEventRepository eventRepository, ISubEventTypeModel eventType)
	{
		BindingContext = new DailyEventsViewModel(eventRepository, eventType);
		InitializeComponent();
	}
	protected override void OnAppearing()
	{
		base.OnAppearing();
		(BindingContext as DailyEventsViewModel).OnAppearing();
	}

}