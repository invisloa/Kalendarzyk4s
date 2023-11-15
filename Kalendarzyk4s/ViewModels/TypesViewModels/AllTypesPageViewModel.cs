﻿using Kalendarzyk4s.Models.EventModels;
using Kalendarzyk4s.Models.EventTypesModels;
using Kalendarzyk4s.Services.DataOperations;
using Kalendarzyk4s.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk4s.ViewModels.TypesViewModels
{

	class AllSubTypesPageViewModel : BaseViewModel
	{
		#region Fields

		private IEventRepository _eventRepository;
		private ObservableCollection<IGeneralEventModel> _allEventsListOC;
		private ObservableCollection<ISubEventTypeModel> _AllSubEventTypesOC;

		#endregion

		#region Properties

		public IEventRepository EventRepository
		{
			get => _eventRepository;
		}

		public ObservableCollection<IGeneralEventModel> AllEventsListOC
		{
			get => _allEventsListOC;
			set
			{
				_allEventsListOC = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<ISubEventTypeModel> AllSubEventTypesOC
		{
			get => _AllSubEventTypesOC;
			set
			{
				if (_AllSubEventTypesOC == value)
				{
					return;
				}
				_AllSubEventTypesOC = value;
				OnPropertyChanged();
			}
		}

		public RelayCommand<ISubEventTypeModel> EditSelectedTypeCommand { get; set; }

		#endregion

		#region Constructor

		public AllSubTypesPageViewModel(IEventRepository eventRepository)
		{
			_eventRepository = eventRepository;
			AllSubEventTypesOC = new ObservableCollection<ISubEventTypeModel>(_eventRepository.AllUserEventTypesList);
			AllEventsListOC = new ObservableCollection<IGeneralEventModel>(_eventRepository.AllEventsList);
			_eventRepository.OnEventListChanged += UpdateAllEventList;
			_eventRepository.OnUserEventTypeListChanged += UpdateAllEventTypesList;
			EditSelectedTypeCommand = new RelayCommand<ISubEventTypeModel>(EditSelectedType);
		}

		#endregion

		#region Public Methods

		public void UpdateAllEventList()
		{
			AllEventsListOC = new ObservableCollection<IGeneralEventModel>(_eventRepository.AllEventsList);
		}

		public void UpdateAllEventTypesList()
		{
			AllSubEventTypesOC = new ObservableCollection<ISubEventTypeModel>(_eventRepository.AllUserEventTypesList);
		}

		#endregion

		#region Private Methods

		private void EditSelectedType(ISubEventTypeModel userTypeToEdit)
		{
			Application.Current.MainPage.Navigation.PushAsync(new AddNewSubTypePage(EventRepository, userTypeToEdit));
		}

		#endregion
	}
}
