using Kalendarzyk4s.Services.DataOperations;
using Kalendarzyk4s.Services.EventsSharing;
using Kalendarzyk4s.ViewModels;
using Kalendarzyk4s.ViewModels.EventsViewModels;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;

namespace Kalendarzyk4s;

public static class MauiProgram
{
	private const string DefaultProgramName = "Kalendarzyk4s";
	private const string DefaultJsonEventsFileName = "CalendarEventsD";
	private const string DefaultJsonUserTypesFileName = "CalendarTypesOfEventsD";

	//statc mauiapp instance to use it for creating DI
	public static MauiApp Current { get; private set; }


	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()

			.UseMauiCommunityToolkit()
// turned off for now... not working- some strange bug..			.UseLocalNotification()

			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("MaterialIcons-Regular.ttf", "GoogleMaterialFont");
			});

		// Interfaces DI Dependency Injection for events repository
		builder.Services.AddSingleton<IEventRepository, LocalMachineEventRepository>();         // events repository DI
		builder.Services.AddScoped<IShareEvents, ShareEventsJson>();
		
		// ViewModels register
		// AddSingleton - one instance for all timne
		// AddTransient - new instance every time
		builder.Services.AddTransient<AddNewSubTypePageViewModel>();
		builder.Services.AddTransient<AddNewMainTypePageViewModel>();
		builder.Services.AddTransient<MonthlyEventsViewModel>();
		builder.Services.AddTransient<WeeklyEventsViewModel>();
		builder.Services.AddTransient<DailyEventsViewModel>();
		builder.Services.AddTransient<AllEventsViewModel>();
		builder.Services.AddTransient<ValueTypeCalculationsViewModel>();


		// add event dictionary factories DI
		//builder.Services.AddSingleton(eventFactories);

		// Preferences Setting General Properties
		Preferences.Default.Set("ProgramName", "Kalendarzyk4s");
		Preferences.Default.Set("JsonEventsFileName", "CalendarEvents");
		Preferences.Default.Set("JsonSubTypesFileName", "CalendarSubTypesOfEvents");
		Preferences.Default.Set("JsonMainTypesFileName", "CalendarMainTypesOfEvents");


#if DEBUG
		builder.Logging.AddDebug();
#endif

		//statc mauiapp instance to use it for creating DI
		Current = builder.Build();

		return Current;
	}
}
