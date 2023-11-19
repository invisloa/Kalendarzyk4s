﻿using Kalendarzyk4s.Helpers;
using Kalendarzyk4s.Models;
using Kalendarzyk4s.Models.EventTypesModels;
using Kalendarzyk4s.Services;
using Kalendarzyk4s.Services.DataOperations;
using Kalendarzyk4s.Views;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk4s.ViewModels
{
	public class AddNewMainTypePageViewModel : BaseViewModel
	{
		private readonly IEventRepository _eventRepository;
		private Dictionary<string, ObservableCollection<string>> _stringToOCMapper;
		private IMainEventType _currentMainType;
		private string _mainTypeName;
		private string _selectedVisualElementString;
		private Color _backgroundColor = Color.FromArgb("#fff");
		private Color _textColor = Color.FromArgb("#000");
		private bool _isEdit;
		private Dictionary<string, RelayCommand<SelectableButtonViewModel>> iconCommandsDictionary;
		private string lastSelectedIconType = "Top";
		public ObservableCollection<SelectableButtonViewModel> BgColorsButtonsOC { get; set; }
		public ObservableCollection<SelectableButtonViewModel> TextColorsButtonsOC { get; set; }

		public Color TESTBUTTONCOLOR
		{ get => Colors.Red; }
		public string MyTestFont { get; set; } = IconFont.Home_filled;

		public ObservableCollection<SelectableButtonViewModel> MainButtonVisualsSelectors { get; set; }
		public ObservableCollection<SelectableButtonViewModel> IconsTabsOC { get; set; }

		public string SubmitMainTypeButtonText => _isEdit ? "SUBMIT CHANGES" : "ADD NEW MAIN TYPE";
		public string MainTypePlaceholderText => _isEdit ? $"TYPE NEW NAME FOR: {MainTypeName}" : "...NEW MAIN TYPE NAME...";


		#region Properties
		public string MainTypeName
		{
			get => _mainTypeName;
			set
			{
				_mainTypeName = value;
				OnPropertyChanged();
				SubmitAsyncMainTypeCommand.NotifyCanExecuteChanged();
			}
		}
		public bool IsEdit
		{
			get => _isEdit;
			set
			{
				_isEdit = value;
				OnPropertyChanged();
			}
		}
		public Color TextColor
		{
			get => _textColor;
			set
			{
				_textColor = value;
				OnPropertyChanged();
			}
		}
		public Color BackgroundColor
		{
			get => _backgroundColor;
			set
			{
				_backgroundColor = value;
				OnPropertyChanged();
			}
		}
		public string SelectedVisualElementString
		{
			get => _selectedVisualElementString;
			set
			{
				_selectedVisualElementString = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<string> IconsToShowStringsOC { get; set; }
		public RelayCommand GoToAllMainTypesPageCommand { get; set; }
		public RelayCommand<string> ExactIconSelectedCommand { get; set; }
		public AsyncRelayCommand SubmitAsyncMainTypeCommand { get; set; }
		public AsyncRelayCommand DeleteAsyncSelectedMainEventTypeCommand { get; set; }
		public RelayCommand<SelectableButtonViewModel> BgColorsCommand { get; private set; }
		public RelayCommand<SelectableButtonViewModel> TextColorsCommand { get; private set; }
		#endregion


		#region Constructors
		//Constructor for create mode
		public AddNewMainTypePageViewModel(IEventRepository eventRepository)
		{
			IsEdit = false;
			_eventRepository = eventRepository;
			InitializeCommon();
		}
		//Constructor for edit mode
		public AddNewMainTypePageViewModel(IEventRepository eventRepository, IMainEventType currentMainType)
		{
			IsEdit = true;
			_eventRepository = eventRepository;
			InitializeCommon();
			_currentMainType = currentMainType;
			MainTypeName = currentMainType.Title;
			SelectedVisualElementString = currentMainType.SelectedVisualElement.ElementName;
			BackgroundColor = currentMainType.SelectedVisualElement.BackgroundColor;
			TextColor = currentMainType.SelectedVisualElement.TextColor;

		}

		#endregion

		#region public methods

		#endregion

		#region private methods
		private void InitializeCommon()
		{
			BgColorsButtonsOC = SelectableButtonHelper.GenerateColorPaletteButtons();
			TextColorsButtonsOC = SelectableButtonHelper.GenerateColorPaletteButtons();
			BgColorsCommand = new RelayCommand<SelectableButtonViewModel>(OnBgColorSeletctionCommand);
			TextColorsCommand = new RelayCommand<SelectableButtonViewModel>(OnTextColorSeletctionCommand);
			RefreshIconsToShowOC();
			InitializeCommands();
			InitializeSelectors();
		}
		private void InitializeIconsTabs()
		{
			IconsTabsOC = new ObservableCollection<SelectableButtonViewModel>
			{
				new SelectableButtonViewModel("Top", true, new RelayCommand(() => OnExactIconsTabCommand("Top"))),
				new SelectableButtonViewModel("Activities", false, new RelayCommand(() => OnExactIconsTabCommand("Activities"))),
				new SelectableButtonViewModel("Others", false, new RelayCommand(() => OnExactIconsTabCommand("Others"))),
			};
			RefreshIconsToShowOC();
			OnPropertyChanged(nameof(IconsTabsOC));
		}
		private void RefreshIconsToShowOC()
		{
			_stringToOCMapper = new Dictionary<string, ObservableCollection<string>>
			{
				{ "Top", IconsHelperClass.GetTopIcons3() },
				{ "Activities", IconsHelperClass.GetTopIcons() },
				{ "Others", IconsHelperClass.GetTopIcons2() }
			};
		}
		private void InitializeCommands()
		{
			GoToAllMainTypesPageCommand = new RelayCommand(OnGoToAllMainTypesPageCommand);
			SubmitAsyncMainTypeCommand = new AsyncRelayCommand(OnSubmitMainTypeCommand, CanExecuteSubmitMainTypeCommand);
			DeleteAsyncSelectedMainEventTypeCommand = new AsyncRelayCommand(OnDeleteMainTypeCommand);
			ExactIconSelectedCommand = new RelayCommand<string>(OnExactIconSelectedCommand);
		}

		private void InitializeSelectors()		// TODO CHANGE THIS TO DYNAMIC LIST !!!!!
		{
			SelectedVisualElementString = IconFont.Minor_crash;
			MainButtonVisualsSelectors = new ObservableCollection<SelectableButtonViewModel>
			{
				new SelectableButtonViewModel("Icons", false, new RelayCommand<SelectableButtonViewModel>(OnShowIconsTabCommand)),
				new SelectableButtonViewModel("Background Colors", false, new RelayCommand<SelectableButtonViewModel>(OnShowBgColorsCommand)),
				new SelectableButtonViewModel("Text Colors", false, new RelayCommand<SelectableButtonViewModel>(OnShowTextColorsCommand)),
			};
			//InitializeIconsTabs();
		}

		private async Task OnSubmitMainTypeCommand()
		{
			var iconForMainEventType = Factory.CreateIMainTypeVisualElement(SelectedVisualElementString, BackgroundColor, TextColor);
			if (_isEdit)
			{
				var x = _eventRepository.AllMainEventTypesList.Single(x => x.Equals(_currentMainType));
				_currentMainType.Title = MainTypeName;
				_currentMainType.SelectedVisualElement = iconForMainEventType;
				MainTypeName = string.Empty;
				x = _currentMainType;
				await _eventRepository.UpdateMainEventTypeAsync(_currentMainType);
				await Shell.Current.GoToAsync("..");    // TODO CHANGE NOT WORKING!!!
			}
			else
			{
				var newMainType = Factory.CreateNewMainEventType(MainTypeName, iconForMainEventType);
				MainTypeName = string.Empty;
				await _eventRepository.AddMainEventTypeAsync(newMainType);
				await Shell.Current.GoToAsync("..");    // TODO !!!!! CHANGE NOT WORKING!!!
			}
		}
		private void OnGoToAllMainTypesPageCommand()
		{
			Application.Current.MainPage.Navigation.PushAsync(new AllMainTypesPage());
		}
		private async Task OnDeleteMainTypeCommand()
		{
			var eventTypesInDb = _eventRepository.AllEventsList.Where(x => x.EventType.MainEventType.Equals(_currentMainType)); // to check
			if (eventTypesInDb.Any())
			{
				var action = await App.Current.MainPage.DisplayActionSheet("This main type is used...", "Cancel", null, "Delete all associated data", "\n", "Go to All SubTypes Page");
				switch (action)
				{
					case "Delete all associated data":
						// Perform the operation to delete all events of the event type.
						await DeleteMainEventType();
						await Shell.Current.GoToAsync("..");

						// TODO make a confirmation message
						break;
					case "Go to All SubTypes Page":
						// Redirect to the All Events Page.
						await Shell.Current.GoToAsync("AllSubTypesPage");   // TODO SELECT PROPPER MAINEVENTTYPE FOR THE PAGE
						break;
					default:
						// Cancel was selected or back button was pressed.
						break;
				}
				return;
			}
			else
			{
				await DeleteMainEventType();
				await Shell.Current.GoToAsync("..");

			}


		}
		private bool CanExecuteSubmitMainTypeCommand()
		{
			return !string.IsNullOrEmpty(MainTypeName);
		}

		private void OnExactIconsTabCommand(string iconType)
		{
			var lastSelectedButton = IconsTabsOC.Single(x => x.ButtonText == iconType);
			OnExactIconsTabClick(lastSelectedButton, _stringToOCMapper[iconType]);
		}
		private void OnExactIconsTabClick(SelectableButtonViewModel clickedButton, ObservableCollection<string> iconsToShowOC)
		{
			SelectableButtonViewModel.SingleButtonSelection(clickedButton, IconsTabsOC);
			lastSelectedIconType = clickedButton.ButtonText;
			IconsToShowStringsOC = iconsToShowOC;
			OnPropertyChanged(nameof(IconsToShowStringsOC));
		}
		private async Task DeleteMainEventType()
		{
			// Perform the operation to delete all events of the event type.
			_eventRepository.AllEventsList.RemoveAll(x => x.EventType.MainEventType.Equals(_currentMainType));
			await _eventRepository.SaveEventsListAsync();
			_eventRepository.AllUserEventTypesList.RemoveAll(x => x.MainEventType.Equals(_currentMainType));
			await _eventRepository.SaveSubEventTypesListAsync();
			_eventRepository.AllMainEventTypesList.Remove(_currentMainType);
			await _eventRepository.SaveMainEventTypesListAsync();
		}


		private void OnExactIconSelectedCommand(string visualStringSource)
		{
			SelectedVisualElementString = visualStringSource;
		}
		#endregion

		private void OnShowIconsTabCommand(SelectableButtonViewModel clickedButton)
		{
			SelectableButtonViewModel.SingleButtonSelection(clickedButton, MainButtonVisualsSelectors);
			InitializeIconsTabs();
			var buttonToSelect = IconsTabsOC.Single(x => x.ButtonText == lastSelectedIconType);
			OnExactIconsTabClick(buttonToSelect, _stringToOCMapper[lastSelectedIconType]);

		}
		private void ClearIconsTabs()
		{
			if (IconsTabsOC != null && IconsTabsOC.Any())
			{
				IconsTabsOC.Clear();
			}
			if (IconsToShowStringsOC != null && IconsToShowStringsOC.Any())
			{
				IconsToShowStringsOC.Clear();
			}
		}

		#region COLOR BUTTONS
		private void OnShowBgColorsCommand(SelectableButtonViewModel clickedButton)
		{
			ClearIconsTabs();
			SelectableButtonViewModel.SingleButtonSelection(clickedButton, MainButtonVisualsSelectors);
		}
		private void OnShowTextColorsCommand(SelectableButtonViewModel clickedButton)
		{
			ClearIconsTabs();
			SelectableButtonViewModel.SingleButtonSelection(clickedButton, MainButtonVisualsSelectors);
		}

		private void OnTextColorSeletctionCommand(SelectableButtonViewModel clickedButton)
		{
			TextColor = clickedButton.ButtonColor;
		}
		private void OnBgColorSeletctionCommand(SelectableButtonViewModel clickedButton)
		{
			BackgroundColor = clickedButton.ButtonColor;
		}
		#endregion
	}
}