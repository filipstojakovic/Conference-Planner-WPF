using System;

namespace ConferenceApp.model.entity
{
    public class Session
    {
        public int? Id { get; set; }
        public int? GatheringId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Session()
        {
            Name = "";
            Description = "";
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddDays(1);
        }
    }
}