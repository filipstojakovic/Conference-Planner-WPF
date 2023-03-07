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

        public Session(Session session)
        {
            copy(session);
        }

        public void copy(Session session)
        {
            Id = session.Id;
            GatheringId = session.GatheringId;
            Name = session.Name;
            Description = session.Description;
            StartDate = session.StartDate;
            EndDate = session.EndDate;
        }
    }
}