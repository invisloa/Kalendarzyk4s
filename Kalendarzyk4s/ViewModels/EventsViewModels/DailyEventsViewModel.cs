using Kalendarzyk4s.Models.EventModels;
using Kalendarzyk4s.Models.EventTypesModels;
using Kalendarzyk4s.Services.DataOperations;
using Kalendarzyk4s.Views.CustomControls.CCInterfaces;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Kalendarzyk4s.ViewModels.EventsViewModels
{
	public class DailyEventsViewModel : AbstractEventViewModel
	{
		private ISubEventTypeModel _eventType;
		public string AboveEventsListText
		{
			get
			{
				// switch selectedLanguage()
				{
					return "EVENTS LIST";
				}
			}
		}
		public DailyEventsViewModel(IEventRepository eventRepository) : base(eventRepository)
		{

		}

		public DailyEventsViewModel(IEventRepository eventRepository, ISubEventTypeModel eventType) : base(eventRepository)
		{
		}
		protected override void ApplyEventsDatesFilter(DateTime startDate, DateTime endDate)
		{

			var selectedToFilterEventTypes = AllSubEventTypesOC
				.Where(x => x.IsSelectedToFilter)
				.Select(x => x.EventTypeName)
				.ToHashSet();

			List<IGeneralEventModel> filteredEvents = AllEventsListOC
				.Where(x => selectedToFilterEventTypes.Contains(x.EventType.ToString()) &&
							x.StartDateTime.Date == startDate.Date &&
							x.EndDateTime.Date <= endDate.Date)
				.ToList();

			// Clear existing items in the EventsToShowList
			EventsToShowList.Clear();

			// Add filtered items to the EventsToShowList
			foreach (var eventItem in filteredEvents)
			{
				EventsToShowList.Add(eventItem);
			}
		}
		public override void BindDataToScheduleList()
		{
			ApplyEventsDatesFilter(CurrentSelectedDate.Date, DateTime.MaxValue);
		}

		public void OnAppearing()
		{

			BindDataToScheduleList();
			EventsToShowList = new ObservableCollection<IGeneralEventModel>(EventsToShowList);
		}




	}
}
