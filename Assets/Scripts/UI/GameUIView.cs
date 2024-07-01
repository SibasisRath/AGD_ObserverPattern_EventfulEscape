using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIView : MonoBehaviour
{
    [Header("Player Sanity")]
    [SerializeField] GameObject rootViewPanel;
    [SerializeField] Image insanityImage;
    [SerializeField] Image redVignette;

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
        EventService.Instance.OnLightsOffByGhostEvent.AddListener(setRedVignette);
        EventService.Instance.PlayerEscapedEvent.AddListener(OnPlayerEscaped);
        EventService.Instance.PlayerDeathEvent.AddListener(setRedVignette);
        EventService.Instance.PlayerDeathEvent.AddListener(OnPlayerDeath);
        EventService.Instance.OnRatRush.AddListener(setRedVignette);
        EventService.Instance.OnSkullDrop.AddListener(setRedVignette);

        tryAgainButton.onClick.AddListener(onTryAgainButtonClicked);
        quitButton.onClick.AddListener(onQuitButtonClicked);
    }
    private void OnDisable()
    {
        EventService.Instance.OnKeyPickedUp.RemoveListener(OnKeyEquipped);
        EventService.Instance.OnLightsOffByGhostEvent.RemoveListener(setRedVignette);
        EventService.Instance.PlayerEscapedEvent.RemoveListener(OnPlayerEscaped);
        EventService.Instance.PlayerDeathEvent.RemoveListener(setRedVignette);
        EventService.Instance.PlayerDeathEvent.RemoveListener(OnPlayerDeath);
        EventService.Instance.OnRatRush.RemoveListener(setRedVignette);
        EventService.Instance.OnSkullDrop.RemoveListener(setRedVignette);
    }

    public void UpdateInsanity(float playerSanity) => insanityImage.rectTransform.localScale = new Vector3(1, playerSanity, 1);

    private void OnKeyEquipped(int keys) => keysFoundText.SetText($"Keys Found: {keys}/ 3");
    private void onQuitButtonClicked() => Application.Quit();
    private void onTryAgainButtonClicked() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


    //Assignment - Call this method as a lister of LightsOffByGhostEvent
    private void setRedVignette()
    {
        redVignette.enabled = true;
        redVignette.canvasRenderer.SetAlpha(0.5f);
        redVignette.CrossFadeAlpha(0, 5, false);
    }

    private void OnPlayerEscaped()
    {

    }

    private void OnPlayerDeath()
    {

    }
}

