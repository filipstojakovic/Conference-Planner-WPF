using System;

namespace ConferenceApp.model.entity
{
	public class Conference : Gathering
	{
		public string Name { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
	}
}
