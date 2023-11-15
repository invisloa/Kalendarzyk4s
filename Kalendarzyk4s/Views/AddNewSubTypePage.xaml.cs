using Kalendarzyk4s.Helpers;
using Kalendarzyk4s.Models.EventTypesModels;
using Kalendarzyk4s.Services.DataOperations;
using Kalendarzyk4s.ViewModels;

using Kalendarzyk4s.ViewModels.EventOperations;
using Microsoft.Maui.Controls.PlatformConfiguration;

namespace Kalendarzyk4s.Views;

public partial class AddNewSubTypePage : ContentPage
{
	public AddNewSubTypePage()
	{
		BindingContext = ServiceHelper.GetService<AddNewSubTypePageViewModel>();
		InitializeComponent();

	}
	public AddNewSubTypePage(IEventRepository eventRepository, ISubEventTypeModel userEventTypeModel)   // edit mode
	{
		BindingContext = new AddNewSubTypePageViewModel(eventRepository, userEventTypeModel);
		InitializeComponent();

	}

}
