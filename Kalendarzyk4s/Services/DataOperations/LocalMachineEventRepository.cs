﻿using Kalendarzyk4s.Models.EventModels;
using Kalendarzyk4s.Models.EventTypesModels;
using Kalendarzyk4s.Services.DataOperations;
using Newtonsoft.Json;
using Microsoft.Maui;
using CommunityToolkit.Maui.Storage;
using System.Text;
using CommunityToolkit.Maui.Alerts;
using Kalendarzyk4s;
using Kalendarzyk4s.Services;
using System.Security.Cryptography;

public class LocalMachineEventRepository : IEventRepository
{
	AdvancedEncryptionStandardService _aesService;


	// File Paths generation code
	#region File Paths generation code
	private static string _eventsFilePath = null;
	private static string _subEventsTypesFilePath = null;
	private static string _mainEventsTypesFilePath = null;

	public event Action OnEventListChanged;
	public event Action OnMainEventTypesListChanged;    // TODO - implement
	public event Action OnUserEventTypeListChanged;
	private static string EventsFilePath
	{
		get
		{
			if (_eventsFilePath == null)
			{
				_eventsFilePath = CalculateEventsFilePath();
			}
			return _eventsFilePath;
		}
	}

	private static string SubEventsTypesFilePath
	{
		get
		{
			if (_subEventsTypesFilePath == null)
			{
				_subEventsTypesFilePath = CalculateSubEventsTypesFilePath();
			}
			return _subEventsTypesFilePath;
		}
	}
	private static string MainEventsTypesFilePath
	{
		get
		{
			if (_mainEventsTypesFilePath == null)
			{
				_mainEventsTypesFilePath = CalculateMainEventsTypesFilePath();
			}
			return _mainEventsTypesFilePath;
		}
	}

	private static string CalculateEventsFilePath()
	{
		return Path.Combine(FileSystem.Current.AppDataDirectory, Preferences.Default.Get("JsonEventsFileName", "CalendarEventsD"));
	}

	private static string CalculateSubEventsTypesFilePath()
	{
		return Path.Combine(FileSystem.Current.AppDataDirectory, Preferences.Default.Get("JsonUserTypesFileName", "CalendarSubTypesOfEventsD"));
	}
	private static string CalculateMainEventsTypesFilePath()
	{
		return Path.Combine(FileSystem.Current.AppDataDirectory, Preferences.Default.Get("JsonMainTypesFileName", "CalendarMainTypesOfEventsD"));
	}
	#endregion

	//CTOR
	public LocalMachineEventRepository()
	{
		// Encryption key and IV
		string keyBase64 = Convert.ToBase64String(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F });
		string ivBase64 = "MojeSuperHasloXD";
		_aesService = new AdvancedEncryptionStandardService(keyBase64, ivBase64);
	}

	#region Events Repository
	private List<IGeneralEventModel> _allEventsList = new List<IGeneralEventModel>();
	public List<IGeneralEventModel> AllEventsList
	{
		get
		{
			return _allEventsList;
		}
		private set
		{
			if (_allEventsList == value) { return; }
			_allEventsList = value;
			OnEventListChanged?.Invoke();
		}
	}
	private List<IMainEventType> _allMainEventTypesList = new List<IMainEventType>();
	public List<IMainEventType> AllMainEventTypesList
	{
		get
		{
			return _allMainEventTypesList;
		}
		private set
		{
			if (_allMainEventTypesList == value) { return; }
			_allMainEventTypesList = value;
			OnMainEventTypesListChanged?.Invoke();
		}
	}
	public async Task AddMainEventTypeAsync(IMainEventType mainEventTypeToAdd)
	{
		if (AllMainEventTypesList.Contains(mainEventTypeToAdd))
		{
			var action = await App.Current.MainPage.DisplayActionSheet($"Event {mainEventTypeToAdd.Title} already exists", "Cancel", null, "Overwrite", "Duplicate");
			switch (action)
			{
				case "Overwrite":
					var eventItem = AllMainEventTypesList.FirstOrDefault(e => e.Equals(mainEventTypeToAdd));            // to check
					if (eventItem != null)
					{
						AllMainEventTypesList.Remove(eventItem);
						AllMainEventTypesList.Add(eventItem);
					}
					break;
				case "Duplicate":
					mainEventTypeToAdd.Title += " (.)";
					AllMainEventTypesList.Add(mainEventTypeToAdd);
					break;

				default:
					// Cancel was selected or back button was pressed.
					return;
			}
		}
		else
		{
			AllMainEventTypesList.Add(mainEventTypeToAdd);
		}
		OnMainEventTypesListChanged?.Invoke();
		await SaveMainEventTypesListAsync();
	}
	public async Task AddEventAsync(IGeneralEventModel eventToAdd)
	{
		AllEventsList.Add(eventToAdd);
		await SaveEventsListAsync();
		OnEventListChanged?.Invoke();
	}
	public async Task ClearAllEventsListAsync()
	{
		AllEventsList.Clear();
		await SaveEventsListAsync();
		OnEventListChanged?.Invoke();

	}
	public async Task ClearAllSubEventTypesAsync()
	{
		await ClearAllEventsListAsync();
		AllUserEventTypesList.Clear();
		await SaveSubEventTypesListAsync();
		OnUserEventTypeListChanged?.Invoke();
	}
	public async Task ClearAllMainEventTypesAsync()
	{

		AllMainEventTypesList.Clear();
		await SaveMainEventTypesListAsync();
		OnMainEventTypesListChanged?.Invoke();
	}
	public async Task<List<IGeneralEventModel>> GetEventsListAsync()
	{
		if (File.Exists(EventsFilePath))
		{
			var jsonString = await File.ReadAllTextAsync(EventsFilePath);
			var settings = JsonSerializerSettings_Auto;
			AllEventsList = JsonConvert.DeserializeObject<List<IGeneralEventModel>>(jsonString, settings);
		}
		else
		{
			AllEventsList = new List<IGeneralEventModel>();
		}
		return AllEventsList;
	}

	public async Task SaveEventsListAsync()
	{
		try
		{
			var directoryPath = Path.GetDirectoryName(EventsFilePath);
			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
			}
			var settings = JsonSerializerSettings_Auto;
			if (AllEventsList.Count > 0)
			{
				AllEventsList = AllEventsList.OrderBy(e => e.StartDateTime).ToList();
			}
			var jsonString = JsonConvert.SerializeObject(AllEventsList, settings);
			await File.WriteAllTextAsync(EventsFilePath, jsonString);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message + "while SaveEventsListAsync");
		}
	}
	public Task<IGeneralEventModel> GetEventByIdAsync(Guid eventId)
	{
		var selectedEvent = AllEventsList.FirstOrDefault(e => e.Id == eventId);
		return Task.FromResult(selectedEvent);
	}
	#endregion


	// UserTypes Repository
	#region UserTypes Repository
	private List<ISubEventTypeModel> _allUserEventTypesList = new List<ISubEventTypeModel>();
	public List<ISubEventTypeModel> AllUserEventTypesList
	{
		get
		{
			return _allUserEventTypesList;
		}
		private set
		{
			if (_allUserEventTypesList == value) { return; }
			_allUserEventTypesList = value;
			OnUserEventTypeListChanged?.Invoke();
		}
	}
	public async Task InitializeAsync()
	{
		_allEventsList = await GetEventsListAsync();                          // TO CHECK -  ConfigureAwait
		_allUserEventTypesList = await GetSubEventTypesListAsync();          // TO CHECK -  ConfigureAwait
		_allMainEventTypesList = await GetMainEventTypesListAsync();          // TO CHECK -  ConfigureAwait
	}
	public async Task<List<IMainEventType>> GetMainEventTypesListAsync()
	{
		if (File.Exists(MainEventsTypesFilePath))
		{
			var jsonString = await File.ReadAllTextAsync(MainEventsTypesFilePath);
			var settings = JsonSerializerSettings_Auto;
			AllMainEventTypesList = JsonConvert.DeserializeObject<List<IMainEventType>>(jsonString, settings);
		}
		else
		{
			AllMainEventTypesList = new List<IMainEventType>();
		}
		return AllMainEventTypesList;
	}
	public async Task<List<ISubEventTypeModel>> GetSubEventTypesListAsync()
	{
		if (File.Exists(SubEventsTypesFilePath))
		{
			var jsonString = await File.ReadAllTextAsync(SubEventsTypesFilePath);
			var settings = JsonSerializerSettings_Auto;
			AllUserEventTypesList = JsonConvert.DeserializeObject<List<ISubEventTypeModel>>(jsonString, settings);
		}
		else
		{
			AllUserEventTypesList = new List<ISubEventTypeModel>();
		}
		return AllUserEventTypesList;
	}

	public async Task SaveSubEventTypesListAsync()
	{
		var directoryPath = Path.GetDirectoryName(SubEventsTypesFilePath);
		if (!Directory.Exists(directoryPath))
		{
			Directory.CreateDirectory(directoryPath);
		}
		var settings = JsonSerializerSettings_Auto;
		var jsonString = JsonConvert.SerializeObject(AllUserEventTypesList, settings);
		await File.WriteAllTextAsync(SubEventsTypesFilePath, jsonString);
	}
	public async Task SaveMainEventTypesListAsync()
	{
		var directoryPath = Path.GetDirectoryName(MainEventsTypesFilePath);
		if (!Directory.Exists(directoryPath))
		{
			Directory.CreateDirectory(directoryPath);
		}
		var settings = JsonSerializerSettings_Auto;
		var jsonString = JsonConvert.SerializeObject(AllMainEventTypesList, settings);
		await File.WriteAllTextAsync(MainEventsTypesFilePath, jsonString);
	}
	public async Task DeleteFromEventsListAsync(IGeneralEventModel eventToDelete)
	{
		AllEventsList.Remove(eventToDelete);
		await SaveEventsListAsync();
		OnEventListChanged?.Invoke();
	}
	public async Task DeleteFromMainEventTypesListAsync(IMainEventType mainEventTypeToDelete)
	{
		AllMainEventTypesList.Remove(mainEventTypeToDelete);
		await SaveMainEventTypesListAsync();
		OnMainEventTypesListChanged?.Invoke();
	}
	public async Task DeleteFromSubEventTypesListAsync(ISubEventTypeModel eventTypeToDelete)
	{
		AllUserEventTypesList.Remove(eventTypeToDelete);
		await SaveSubEventTypesListAsync();
		OnUserEventTypeListChanged?.Invoke();
	}

	public async Task AddSubEventTypeAsync(ISubEventTypeModel eventTypeToAdd)
	{
		AllUserEventTypesList.Add(eventTypeToAdd);
		OnUserEventTypeListChanged?.Invoke();
		await SaveSubEventTypesListAsync();
	}
	public async Task UpdateEventAsync(IGeneralEventModel eventToUpdate)   // cos nie tak	???
	{
		var eventToUpdateInList = AllEventsList.FirstOrDefault(e => e.Id == eventToUpdate.Id);
		if (eventToUpdateInList != null)
		{
			// TO CHECK
			await SaveEventsListAsync();
		}
		else
		{
			await Task.CompletedTask;
		}
	}

	public async Task UpdateSubEventTypeAsync(ISubEventTypeModel eventTypeToUpdate)
	{
		await SaveSubEventTypesListAsync();
		OnUserEventTypeListChanged?.Invoke();
	}
	public async Task UpdateMainEventTypeAsync(IMainEventType eventTypeToUpdate)
	{
		await SaveMainEventTypesListAsync();
		OnMainEventTypesListChanged?.Invoke();
	}
	public Task<ISubEventTypeModel> GetSubEventTypeAsync(ISubEventTypeModel eventTypeToSelect)
	{
		var selectedEventType = AllUserEventTypesList.FirstOrDefault(e => e.Equals(eventTypeToSelect));
		return Task.FromResult(selectedEventType);
	}
	public Task<IMainEventType> GetMainEventTypeAsync(IMainEventType eventTypeToSelect)
	{
		var selectedEventType = AllMainEventTypesList.FirstOrDefault(e => e.Equals(eventTypeToSelect));
		return Task.FromResult(selectedEventType);
	}
	public List<IGeneralEventModel> DeepCopyAllEventsList()
	{
		var settings = JsonSerializerSettings_Auto;
		var serialized = JsonConvert.SerializeObject(_allEventsList, settings);
		return JsonConvert.DeserializeObject<List<IGeneralEventModel>>(serialized, settings);
	}
	public List<ISubEventTypeModel> DeepCopySubEventTypesList()
	{
		var settings = JsonSerializerSettings_Auto;
		var serialized = JsonConvert.SerializeObject(_allUserEventTypesList, settings);
		return JsonConvert.DeserializeObject<List<ISubEventTypeModel>>(serialized, settings);
	}
	public List<IMainEventType> DeepCopyMainEventTypesList()
	{
		var settings = JsonSerializerSettings_Auto;
		var serialized = JsonConvert.SerializeObject(_allMainEventTypesList, settings);
		return JsonConvert.DeserializeObject<List<IMainEventType>>(serialized, settings);
	}
	#endregion

	//FILE SAVE AND LOAD EVENTS AND TYPES
	#region FILE SAVE AND LOAD
	async Task SaveEventsAndTypesToFile(CancellationToken cancellationToken, List<IGeneralEventModel> eventsToSaveList = null)
	{
		var settings = JsonSerializerSettings_All;
		EventsAndTypesForJson eventsAndTypesToSave;
		// if eventsToSaveList is null, save all events and types
		if (eventsToSaveList == null)
		{
			eventsAndTypesToSave = new EventsAndTypesForJson()
			{
				Events = AllEventsList,
				UserEventTypes = AllUserEventTypesList,
				MainEventTypes = AllMainEventTypesList
			};
		}
		else
		{
			var subTypesToSaveFromSpecifiedEvents = new List<ISubEventTypeModel>();
			var mainTypesToSaveFromSpecifiedEvents = new List<IMainEventType>();

			foreach (var eventItem in eventsToSaveList)
			{
				if (!subTypesToSaveFromSpecifiedEvents.Contains(eventItem.EventType))
				{
					subTypesToSaveFromSpecifiedEvents.Add(eventItem.EventType);
				}
				if (!mainTypesToSaveFromSpecifiedEvents.Contains(eventItem.EventType.MainEventType))
				{
					mainTypesToSaveFromSpecifiedEvents.Add(eventItem.EventType.MainEventType);
				}
			}
			eventsAndTypesToSave = new EventsAndTypesForJson()
			{
				Events = eventsToSaveList,
				UserEventTypes = subTypesToSaveFromSpecifiedEvents,
				MainEventTypes = mainTypesToSaveFromSpecifiedEvents
			};
		}

		try
		{
			var jsonString = JsonConvert.SerializeObject(eventsAndTypesToSave, settings);
			var encryptedString = _aesService.EncryptString(jsonString); // Encrypt the jsonString
			using var stream = new MemoryStream(Encoding.UTF8.GetBytes(encryptedString)); // Use UTF8 or another Encoding as needed.

			var fileSaverResult = await FileSaver.Default.SaveAsync("EventsList.cics", stream, cancellationToken);
			if (fileSaverResult.IsSuccessful)
			{
				await Toast.Make($"The file was saved successfully to location: {fileSaverResult.FilePath}").Show(cancellationToken);
			}
			else
			{
				await Toast.Make($"The file was not saved successfully with error: {fileSaverResult.Exception.Message}").Show(cancellationToken);
			}
		}
		catch (Exception ex)
		{
			await Toast.Make($"The file was not saved successfully with error: {ex.Message}").Show(cancellationToken);
		}
	}


	async Task LoadEventsAndTypesFromFile(CancellationToken cancellationToken)
	{
		var settings = JsonSerializerSettings_All;
		var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
	{
		{ DevicePlatform.WinUI, new[] { ".cics" } },
		{ DevicePlatform.Android, new[] { ".cics" } },
		{ DevicePlatform.iOS, new[] { ".cics" } }
	});
		var pickOptions = new PickOptions
		{
			FileTypes = customFileType
		};
		try
		{
			// Prompt the user to select the file
			var filePickerResult = await FilePicker.PickAsync(pickOptions);

			if (filePickerResult != null)
			{
				using var stream = await filePickerResult.OpenReadAsync();
				using var reader = new StreamReader(stream, Encoding.Default); // Use consistent encoding
				var encryptedString = await reader.ReadToEndAsync();
				var jsonString = _aesService.DecryptString(encryptedString); // Decrypt the string

				// Deserialize the content of the file
				var loadedData = JsonConvert.DeserializeObject<EventsAndTypesForJson>(jsonString, settings);


				foreach (var eventItem in loadedData.Events)
				{
					var isEventAlreadyAdded = AllEventsList.Any(e => e.Id == eventItem.Id);
					if (!isEventAlreadyAdded)
					{
						AllEventsList.Add(eventItem);
					}
					else
					{
						// ask the user if he wants to overwrite the event
						var action = await App.Current.MainPage.DisplayActionSheet($"Event {eventItem.Title} already exists", "Cancel", null, "Overwrite", "Duplicate", "Skip");
						switch (action)
						{
							case "Overwrite":
								var eventToUpdate = AllEventsList.FirstOrDefault(e => e.Id == eventItem.Id);
								if (eventToUpdate != null)
								{
									AllEventsList.Remove(eventToUpdate);
									AllEventsList.Add(eventItem);
								}
								break;
							case "Duplicate":
								eventItem.Id = Guid.NewGuid();
								eventItem.Title += " (.)";
								AllEventsList.Add(eventItem);
								break;
							case "Skip":
								// Do nothing, just skip.
								break;
							default:
								// Cancel was selected or back button was pressed.
								break;
						}
					}
				}

				foreach (var eventType in loadedData.UserEventTypes)
				{
					if (!AllUserEventTypesList.Contains(eventType))
					{
						AllUserEventTypesList.Add(eventType);
					}
					if (!AllMainEventTypesList.Contains(eventType.MainEventType))
					{
						AllMainEventTypesList.Add(eventType.MainEventType);
					}
				}

				await Toast.Make($"Data loaded successfully from: {filePickerResult.FileName}").Show(cancellationToken);
				await SaveEventsListAsync();
				await SaveSubEventTypesListAsync();
				await SaveMainEventTypesListAsync();
			}
			else
			{
				await Toast.Make($"Failed to pick a file: User canceled file picking").Show(cancellationToken);
			}
		}
		catch (Exception ex)
		{
			await Toast.Make($"An error occurred while loading the file: {ex.Message}").Show(cancellationToken);
		}
	}

	private static readonly JsonSerializerSettings JsonSerializerSettings_Auto = new JsonSerializerSettings
	{
		TypeNameHandling = TypeNameHandling.Auto
	};
	private static readonly JsonSerializerSettings JsonSerializerSettings_All = new JsonSerializerSettings
	{
		TypeNameHandling = TypeNameHandling.All
	};

	public async Task SaveEventsAndTypesToFile(List<IGeneralEventModel> eventsToSaveList = null)
	{

		await SaveEventsAndTypesToFile(CancellationToken.None, eventsToSaveList);
	}
	public async Task LoadEventsAndTypesFromFile()
	{
		await LoadEventsAndTypesFromFile(CancellationToken.None);
	}
	private class EventsAndTypesForJson
	{
		public List<IGeneralEventModel> Events { get; set; }
		public List<ISubEventTypeModel> UserEventTypes { get; set; }
		public List<IMainEventType> MainEventTypes { get; set; }
	}


	#endregion
}
