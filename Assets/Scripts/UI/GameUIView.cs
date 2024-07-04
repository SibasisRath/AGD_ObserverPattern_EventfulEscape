using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIView : MonoBehaviour
{
    [Header("Player Sanity")]
    [SerializeField] GameObject rootViewPanel;
    [SerializeField] Image insanityImage;
    [SerializeField] Image redVignette;   // for damage
    [SerializeField] Image whiteVignette; // for healing
    [SerializeField] Image greenVignette; // for achievement

    [Header("Keys UI")]
    [SerializeField] TextMeshProUGUI keysFoundText;

    [Header("Game End Panel")]
    [SerializeField] GameObject gameEndPanel;
    [SerializeField] TextMeshProUGUI gameEndText;
    [SerializeField] Button tryAgainButton;
    [SerializeField] Button quitButton;

    private void OnEnable()
    {
        EventService.Instance.OnKeyPickedUp.AddListener(OnKeyEquipped);
        EventService.Instance.OnLightsOffByGhostEvent.AddListener(SetRedVignette);
        EventService.Instance.PlayerEscapedEvent.AddListener(OnPlayerEscaped);
        //EventService.Instance.PlayerDeathEvent.AddListener(SetRedVignette);
        EventService.Instance.PlayerDeathEvent.AddListener(OnPlayerDeath);
        EventService.Instance.OnRatRush.AddListener(SetRedVignette);
        EventService.Instance.OnSkullDrop.AddListener(SetRedVignette);
        EventService.Instance.OnPotionDrink.AddListener(SetWhiteVignette);
        EventService.Instance.OnAchievement.AddListener(SetGreenVignette);

        tryAgainButton.onClick.AddListener(OnTryAgainButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }
    private void OnDisable()
    {
        EventService.Instance.OnKeyPickedUp.RemoveListener(OnKeyEquipped);
        EventService.Instance.OnLightsOffByGhostEvent.RemoveListener(SetRedVignette);
        EventService.Instance.PlayerEscapedEvent.RemoveListener(OnPlayerEscaped);
       // EventService.Instance.PlayerDeathEvent.RemoveListener(SetRedVignette);
        EventService.Instance.PlayerDeathEvent.RemoveListener(OnPlayerDeath);
        EventService.Instance.OnRatRush.RemoveListener(SetRedVignette);
        EventService.Instance.OnSkullDrop.RemoveListener(SetRedVignette);
        EventService.Instance.OnPotionDrink.RemoveListener(SetWhiteVignette);
        EventService.Instance.OnAchievement.RemoveListener(SetGreenVignette);
    }

    public void UpdateInsanity(float playerSanity) => insanityImage.rectTransform.localScale = new Vector3(1, playerSanity, 1);

    private void OnKeyEquipped(int keys) => keysFoundText.SetText($"Keys Found: {keys}/ 3");
    private void OnQuitButtonClicked() => Application.Quit();
    private void OnTryAgainButtonClicked() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


    //Assignment - Call this method as a lister of LightsOffByGhostEvent
    private void SetRedVignette()
    {
        redVignette.enabled = true;
        redVignette.canvasRenderer.SetAlpha(0.5f);
        redVignette.CrossFadeAlpha(0, 5, false);
    }

    // for potion
    private void SetWhiteVignette(int val)
    {
        whiteVignette.enabled = true;
        whiteVignette.canvasRenderer.SetAlpha(0.5f);
        whiteVignette.CrossFadeAlpha(0, 5, false);
    }

    private void SetGreenVignette()
    {
        greenVignette.enabled = true;
        greenVignette.canvasRenderer.SetAlpha(0.5f);
        greenVignette.CrossFadeAlpha(0, 5, false);
    }

    private void OnPlayerEscaped()
    {
        gameEndPanel.SetActive(true);
        gameEndText.text = "Player Escaped.";
        gameEndText.color = Color.green;
        GameService.Instance.GetInstructionView().HideInstruction();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnPlayerDeath()
    {
        SetRedVignette();
        gameEndPanel.SetActive(true);
        gameEndText.text = "Player died.";
        GameService.Instance.GetInstructionView().HideInstruction();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}

