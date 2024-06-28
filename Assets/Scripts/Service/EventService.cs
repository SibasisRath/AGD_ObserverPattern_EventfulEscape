
public class EventService
{
    private static EventService instance;
    public static EventService Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EventService();
            }
            return instance;
        }
    }

    public EventController OnLightSwitchToggled { get; private set; }
    public EventController<int> OnKeyPickUp { get; private set; }

    public EventService()
    {
        OnLightSwitchToggled = new EventController();
    }
}
