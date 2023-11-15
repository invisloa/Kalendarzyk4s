using Kalendarzyk4s.Helpers;
using Kalendarzyk4s.Models.EventModels;
using Kalendarzyk4s.Services.DataOperations;
using Kalendarzyk4s.ViewModels.EventOperations;
using Kalendarzyk4s.ViewModels.EventsViewModels;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Kalendarzyk4s.Views
{
	public partial class EventPage : ContentPage
	{
		// For adding events
		public EventPage(IEventRepository eventRepository, DateTime selcetedDate)
		{
			InitializeComponent();

			BindingContext = new EventOperationsViewModel(eventRepository, selcetedDate);
		}
		public EventPage()
		{
			var viewModel = ServiceHelper.GetService<EventOperationsViewModel>();
			BindingContext = viewModel;
			InitializeComponent();
		}
		// For editing events
		public EventPage(IEventRepository eventRepository, IGeneralEventModel eventModel)
		{
			InitializeComponent();

			BindingContext = new EventOperationsViewModel(eventRepository, eventToEdit: eventModel);

		}

	}
}
