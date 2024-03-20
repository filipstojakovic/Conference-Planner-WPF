namespace ConferenceApp.model.entity;

public class OnlineEvent: Event
{
    public string Url { get; set; }
    
    public OnlineEvent()
    {
    }

    public OnlineEvent(OnlineEvent liveEvent)
    {
        copy(liveEvent);
    }
    
    public void copy(OnlineEvent _event)
    {
        this.copy(_event);
        Url = _event.Url;
    }
}