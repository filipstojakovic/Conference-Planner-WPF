using System;

namespace ConferenceApp.model.entity
{
    public class Conference : Gathering
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Conference()
        {
        }

        public Conference(Conference conference)
        {
            Id = conference.Id;
            Name = conference.Name;
            Description = conference.Description;
            StartDate = conference.StartDate;
            EndDate = conference.EndDate;
        }
    }
}