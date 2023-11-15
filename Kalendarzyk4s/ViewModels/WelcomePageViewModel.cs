﻿using CalendarT1.Models;
using CalendarT1.Services;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.Views.CustomControls.CCInterfaces;
using CalendarT1.Views.CustomControls.CCViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.ViewModels
{
	internal class WelcomePageViewModel : BaseViewModel
	{
		private IMainEventTypesCCViewModel _mainEventTypesCCHelper;
		private IEventRepository _eventRepository;
		public ObservableCollection<MainEventTypeViewModel> MainEventTypesVisualsOC { get => ((IMainEventTypesCCViewModel)_mainEventTypesCCHelper).MainEventTypesVisualsOC; set => ((IMainEventTypesCCViewModel)_mainEventTypesCCHelper).MainEventTypesVisualsOC = value; }
		public RelayCommand ButtonClickCommand { get; private set; }

		private string someIconString;
		public string SomeIconString
		{
			get
			{
				return someIconString;
			}
			set
			{
				someIconString = value;
				SomeIconToShow = Factory.CreateIMainTypeVisualElement(someIconString, Color.FromArgb("#FF0000"), Color.FromArgb("#FF0000"));
				OnPropertyChanged(nameof(SomeIconString));
			}
		}

		private IMainTypeVisualModel someIconToShow;
		public IMainTypeVisualModel SomeIconToShow
		{
			get
			{
				return someIconToShow;
			}
			set
			{
				someIconToShow = value;
				OnPropertyChanged(nameof(SomeIconToShow));
			}
		}

		public WelcomePageViewModel(IEventRepository eventRepository)
		{
			_eventRepository = eventRepository;
			_mainEventTypesCCHelper = Factory.CreateNewIMainEventTypeViewModelClass(eventRepository.AllMainEventTypesList);
			ButtonClickCommand = new RelayCommand(OnButtonCommand);
		}

		private void OnButtonCommand()
		{
			var y = MainEventTypesVisualsOC[0].MainEventType.SelectedVisualElement.ElementName;
			SomeIconString = y;

		}



	}
}
