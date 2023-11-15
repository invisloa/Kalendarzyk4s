using Kalendarzyk4s.Models.EventModels;

namespace Kalendarzyk4s.Services.EventsSharing
{
	public interface IShareEvents
	{
		public Task ShareEventAsync(IGeneralEventModel eventModel);

		public Task ImportEventAsync(string jsonString);
	}
}
