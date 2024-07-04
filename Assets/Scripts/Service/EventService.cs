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
    public EventController<int> OnKeyPickedUp { get; private set; }
    public EventController OnLightsOffByGhostEvent { get; private set; }

    public EventController PlayerEscapedEvent { get; private set; }
    public EventController PlayerDeathEvent { get; private set; }
    public EventController OnRatRush { get; private set; }
    public EventController OnSkullDrop { get; internal set; }
    public EventController<int> OnPotionDrink { get; internal set; }

    // achievements
    // public EventController OnKeyMasterAchievement { get; internal set; }
    // public EventController OnSanitySaverAchievement { get; internal set; }
    public EventController<float> CheckingTormentedSurvivorAchievement { get; internal set; }
    // public EventController OnTormentedSurvivorAchievement { get; internal set; }
    public EventController StartCheckingForShadowMaster { get; internal set; }
    public EventController StopCheckingForShadowMaster { get; internal set; }
    // public EventController OnMasterOfShadowAchievement { get; internal set; }
    public EventController OnAchievement { get; internal set; }


    public EventService()
    {
        OnLightSwitchToggled = new EventController();
        OnKeyPickedUp = new EventController<int>();
        OnLightsOffByGhostEvent = new EventController();
        OnRatRush = new EventController();
        OnSkullDrop = new EventController();
        OnPotionDrink = new EventController<int>();

        PlayerEscapedEvent = new EventController();
        PlayerDeathEvent = new EventController();

        StartCheckingForShadowMaster = new EventController();
        StopCheckingForShadowMaster = new EventController();
        CheckingTormentedSurvivorAchievement = new EventController<float> ();

        OnAchievement = new EventController();
    }
}
