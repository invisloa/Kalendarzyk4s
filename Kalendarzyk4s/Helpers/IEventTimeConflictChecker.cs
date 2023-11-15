using Kalendarzyk4s.Models.EventModels;

namespace Kalendarzyk4s.Helpers
{
	public interface IEventTimeConflictChecker
	{
		public List<IGeneralEventModel> allEvents { get; set; }

		bool IsEventConflict(bool isSubEventTimeDifferent, bool isMainEventTimeDifferent, IGeneralEventModel eventToCheck);
	}
}