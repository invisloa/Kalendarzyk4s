using Kalendarzyk4s.Helpers;
using Kalendarzyk4s.Services.DataOperations;
using Kalendarzyk4s.ViewModels.TypesViewModels;
using System.Net.WebSockets;

namespace Kalendarzyk4s.Views;

public partial class AllSubTypesPage : ContentPage
{
	public AllSubTypesPage()
	{
		var eventRepository = ServiceHelper.GetService<IEventRepository>();
		BindingContext = new AllSubTypesPageViewModel(eventRepository);
		InitializeComponent();
	}
}