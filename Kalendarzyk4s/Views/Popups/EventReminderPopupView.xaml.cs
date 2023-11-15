using Kalendarzyk4s.Models.EventModels;
using Kalendarzyk4s.Services.DataOperations;
using Kalendarzyk4s.ViewModels.EventOperations;
using CommunityToolkit.Maui.Views;

namespace Kalendarzyk4s.Views;

public partial class EventReminderPopupView : Popup
{

	public EventReminderPopupView(IEventRepository eventRepository, IGeneralEventModel eventToEdit)
	{

		//		BindingContext = new EventReminderPopupViewModel(eventRepository, eventToEdit);
		InitializeComponent();
	}
}