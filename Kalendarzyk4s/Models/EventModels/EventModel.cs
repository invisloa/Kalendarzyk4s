﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kalendarzyk4s.Models.EventTypesModels;
using Kalendarzyk4s.Models.EventModels;

namespace Kalendarzyk4s.Models.EventModels
{
	public class EventModel : AbstractEventModel
	{
		public EventModel(string title, string description, DateTime startTime, DateTime endTime, ISubEventTypeModel EventType, bool isCompleted = false, TimeSpan? postponeTime = null, bool wasShown = false, QuantityModel quantityAmount = null, IEnumerable<MicroTaskModel> microTasksList = null) : base(title, description, startTime, endTime, EventType, isCompleted, postponeTime, wasShown, quantityAmount, microTasksList)
		{
		}
	}
}
