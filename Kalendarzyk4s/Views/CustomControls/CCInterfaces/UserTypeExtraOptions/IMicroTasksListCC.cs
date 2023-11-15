using Kalendarzyk4s.Models.EventModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk4s.Views.CustomControls.CCInterfaces.UserTypeExtraOptions
{
	public interface IMicroTasksCC
	{
		string MicroTaskToAddName { get; set; }
		RelayCommand AddMicroTaskEventCommand { get; set; }
		ObservableCollection<MicroTaskModel> MicroTasksOC { get; set; }
		RelayCommand<MicroTaskModel> SelectMicroTaskCommand { get; set; }
	}
}
