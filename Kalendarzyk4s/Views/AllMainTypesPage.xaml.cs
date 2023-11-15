using Kalendarzyk4s.Helpers;
using Kalendarzyk4s.Services.DataOperations;
using Kalendarzyk4s.ViewModels;
using Kalendarzyk4s.ViewModels.TypesViewModels;

namespace Kalendarzyk4s.Views;

public partial class AllMainTypesPage : ContentPage
{
	IEventRepository _eventRepository;
	public AllMainTypesPage()
	{
		_eventRepository = ServiceHelper.GetService<IEventRepository>();
		BindingContext = new AllMainTypesPageViewModel(_eventRepository);

		InitializeComponent();
	}
}