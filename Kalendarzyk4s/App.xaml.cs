﻿using Kalendarzyk4s.Helpers;
using Kalendarzyk4s.Models;
using Kalendarzyk4s.Models.EventModels;
using Kalendarzyk4s.Models.EventTypesModels;
using Kalendarzyk4s.Services;
using Kalendarzyk4s.Services.DataOperations;
using System.Globalization;

namespace Kalendarzyk4s;

public partial class App : Application
{
	public void ClearData()
	{
		_repository.ClearAllEventsListAsync();
		_repository.ClearAllSubEventTypesAsync();
		_repository.ClearAllMainEventTypesAsync();
	}
	public void AddDummyData()
	{
		/*		if(_repository.AllEventsList.Count > 0)
				{
					return;
				}*/
		IMainTypeVisualModel mainTypeVisualModel = new IconModel(IconFont.Work, Colors.Aquamarine, Colors.AliceBlue);
		IMainEventType mainEventType = new MainEventType("Invioces", mainTypeVisualModel);
		_repository.AddMainEventTypeAsync(mainEventType);

		IMainTypeVisualModel mainTypeVisualModel2 = new IconModel(IconFont.Home, Colors.AliceBlue, Colors.Aquamarine);
		IMainEventType mainEventType2 = new MainEventType("Home", mainTypeVisualModel2);
		_repository.AddMainEventTypeAsync(mainEventType2);

		IMainTypeVisualModel mainTypeVisualModel3 = new IconModel(IconFont.Traffic, Colors.Red, Colors.BlanchedAlmond);
		IMainEventType mainEventType3 = new MainEventType("RoadTrip", mainTypeVisualModel3);
		_repository.AddMainEventTypeAsync(mainEventType3);


		ISubEventTypeModel subEventTypeModel = Factory.CreateNewEventType(mainEventType, "Dino", Colors.Blue, TimeSpan.FromSeconds(0), new QuantityModel(MeasurementUnit.Money, 100));
		ISubEventTypeModel subEventTypeModel2 = Factory.CreateNewEventType(mainEventType, "Chrupki", Colors.Red, TimeSpan.FromSeconds(0), new QuantityModel(MeasurementUnit.Money, 300));
		ISubEventTypeModel subEventTypeModel3 = Factory.CreateNewEventType(mainEventType2, "MicroTasker", Colors.MediumPurple, TimeSpan.FromSeconds(0), null, new List<MicroTaskModel> { new MicroTaskModel("Task1"), new MicroTaskModel("Task2") });
		ISubEventTypeModel subEventTypeModel4 = Factory.CreateNewEventType(mainEventType2, "MicroTasker2", Colors.Purple, TimeSpan.FromSeconds(0), null, new List<MicroTaskModel> { new MicroTaskModel("Task1", false), new MicroTaskModel("Task2", false) });
		ISubEventTypeModel subEventTypeModel5 = new SubEventTypeModel(mainEventType3, "Plain1", Colors.DarkCyan, TimeSpan.FromSeconds(0));
		ISubEventTypeModel subEventTypeModel6 = new SubEventTypeModel(mainEventType3, "Plain2", Colors.DarkGoldenrod, TimeSpan.FromSeconds(0));

		_repository.AddSubEventTypeAsync(subEventTypeModel);
		_repository.AddSubEventTypeAsync(subEventTypeModel2);
		_repository.AddSubEventTypeAsync(subEventTypeModel3);
		_repository.AddSubEventTypeAsync(subEventTypeModel4);
		_repository.AddSubEventTypeAsync(subEventTypeModel5);
		_repository.AddSubEventTypeAsync(subEventTypeModel6);

		_repository.AddEventAsync(Factory.CreatePropperEvent("Dino", "Dino", DateTime.Now, DateTime.Now.AddHours(1), subEventTypeModel, new QuantityModel(MeasurementUnit.Money, 100)));
		_repository.AddEventAsync(Factory.CreatePropperEvent("Chrupki", "Chrupki", DateTime.Now, DateTime.Now.AddHours(1), subEventTypeModel2, new QuantityModel(MeasurementUnit.Money, 300)));
		_repository.AddEventAsync(Factory.CreatePropperEvent("MicroTasker", "MicroTasker", DateTime.Now, DateTime.Now.AddHours(1), subEventTypeModel3));
		_repository.AddEventAsync(Factory.CreatePropperEvent("MicroTasker2", "MicroTasker2", DateTime.Now, DateTime.Now.AddHours(1), subEventTypeModel4));
		_repository.AddEventAsync(Factory.CreatePropperEvent("Plain1", "Plain1", DateTime.Now, DateTime.Now.AddHours(1), subEventTypeModel5));
		_repository.AddEventAsync(Factory.CreatePropperEvent("Plain2", "Plain2", DateTime.Now, DateTime.Now.AddHours(1), subEventTypeModel6));

	}


	private readonly IEventRepository _repository;

	public App(IEventRepository repository)
	{
		_repository = repository;

		//ClearData();

		AddDummyData();

		InitializeComponent();
		MainPage = new AppShell();

	}
	protected override async void OnStart()
	{
		// Call base method 
		base.OnStart();


		// Check or request StorageRead permission
		var statusStorageRead = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
		if (statusStorageRead != PermissionStatus.Granted)
		{
			statusStorageRead = await Permissions.RequestAsync<Permissions.StorageRead>();
		}
		var statusStorageWrite = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
		if (statusStorageWrite != PermissionStatus.Granted)
		{
			statusStorageWrite = await Permissions.RequestAsync<Permissions.StorageRead>();
		}

		// load repository data OnStart of the app
		await _repository.InitializeAsync();
	}
	public static class Styles
	{
		public static Style GoogleFontStyle = new Style(typeof(Label))
		{
			Setters =
		{
			new Setter { Property = Label.FontFamilyProperty, Value = "GoogleMaterialFont" },
			new Setter { Property = Label.FontSizeProperty, Value = 32 }
		}
		};
	}
}
