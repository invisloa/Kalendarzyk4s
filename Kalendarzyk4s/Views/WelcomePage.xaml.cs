using Kalendarzyk4s.Helpers;
using Kalendarzyk4s.Models;
using Kalendarzyk4s.Models.EventTypesModels;
using Kalendarzyk4s.Services;
using Kalendarzyk4s.Services.DataOperations;
using Kalendarzyk4s.ViewModels;

namespace Kalendarzyk4s.Views;

public partial class WelcomePage : ContentPage
{
	IEventRepository _eventRepository;

	public WelcomePage()
	{
		InitializeComponent();
		_eventRepository = ServiceHelper.GetService<IEventRepository>();

		BindingContext = new WelcomePageViewModel(_eventRepository);


		//_eventRepository.ClearAllMainEventTypesAsync();
		//_eventRepository.ClearAllSubEventTypesAsync();
		//_eventRepository.ClearAllEventsListAsync();
	}

	private void Button_Clicked(object sender, EventArgs e)
	{
		var icon = Factory.CreateIMainTypeVisualElement("logout.png", Color.FromArgb("#FF0000"), Color.FromArgb("#FF0000"));
		var icon2 = Factory.CreateIMainTypeVisualElement("login.png", Color.FromArgb("#FFaaaa"), Color.FromArgb("#FFaaaa"));
		var icon3 = Factory.CreateIMainTypeVisualElement("logout.png", Color.FromArgb("#FFdd00"), Color.FromArgb("#FFdd00"));
		IMainEventType userEventTypeModel = new MainEventType("test1", icon);
		_eventRepository.AddMainEventTypeAsync(userEventTypeModel);
		IMainEventType userEventTypeModel2 = new MainEventType("test2", icon2);
		_eventRepository.AddMainEventTypeAsync(userEventTypeModel2);
		IMainEventType userEventTypeModel3 = new MainEventType("test3", icon3);
		_eventRepository.AddMainEventTypeAsync(userEventTypeModel3);


	}
}