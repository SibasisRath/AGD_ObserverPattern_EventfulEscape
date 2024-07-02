using UnityEngine;

public class GameService : GenericMonoSingleton<GameService>
{
    private PlayerController playerController;

    [Header("Views")]
    [SerializeField] private PlayerView playerView;
    [SerializeField] private SoundView soundView;
    [SerializeField] private GameUIView gameUIView;
    [SerializeField] private InstructionView instructionView;

    [Header("Scriptable Objects")]
    [SerializeField] private PlayerScriptableObject playerScriptableObject;

    private void Start() => playerController = new PlayerController(playerView, playerScriptableObject);

    public PlayerController GetPlayerController() => playerController;
    public GameUIView GetGameUI() => gameUIView;
    public InstructionView GetInstructionView() => instructionView;
    public SoundView GetSoundView() => soundView;

    public void GameOver()
    {
        playerController.KillPlayer();
        soundView.PlaySoundEffects(SoundType.JumpScare1);
        EventService.Instance.PlayerDeathEvent.InvokeEvent();
    }

    /*public void RestartGame()
    {
        // Destroy the current PlayerView and reinstantiate it
        Destroy(playerView.gameObject);
        // Assuming you have a way to instantiate the PlayerView, e.g., from a prefab
        playerView = Instantiate(playerViewPrefab);  // playerViewPrefab should be a reference to the PlayerView prefab
        InitializePlayerController();
    }*/
}
