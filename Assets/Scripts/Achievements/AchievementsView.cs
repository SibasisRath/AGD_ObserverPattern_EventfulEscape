using System.Collections;
using TMPro;
using UnityEngine;

public class AchievementsView : MonoBehaviour
{
    [Header("Achievents SO")]
    [SerializeField] private AchievementSO keyMaster;
    [SerializeField] private AchievementSO sanitySaver;
    [SerializeField] private AchievementSO tormentedSurvivor;
    [SerializeField] private AchievementSO masterOfShadow;

    [Header("Achievement Popup")]
    [SerializeField] private GameObject achievementPopup;
    [SerializeField] private TextMeshProUGUI achievementText;

    [Header("Achievents")]
    [Header("KeyMaster")]
    [SerializeField] private int maxNuberOfKeys = 3;
    [Header("Sanity Saver")]
    [SerializeField] private int maxNumberOfPotion = 2;
    [Header("Tormented Survivor")]
    [SerializeField] private float tormentedSurvivorLimit = 90f;
    [Header("Master Of Shadow")]
    [SerializeField] private float masterOfShadowTimeLimit = 120f;

    private int numberOfPotionsConsumed = 0;
    private float shadowMasterCounter;

    private Coroutine shadowMasterCoroutine;
    private Coroutine achievementCoroutine;

    public int MaxNuberOfKeys => maxNuberOfKeys;
    public int MaxNumberOfPotion => maxNumberOfPotion;
    public float TormentedSurvivorLimit => tormentedSurvivorLimit;
    public float MasterOfShadowTimeLimit => masterOfShadowTimeLimit;

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        EventService.Instance.OnKeyPickedUp.AddListener(UpdateKeyValue);
        EventService.Instance.OnPotionDrink.AddListener(UpdateNumberOfPotionsConsumed);
        EventService.Instance.StartCheckingForShadowMaster.AddListener(CheckForShadowMasterAchievement);
        EventService.Instance.StopCheckingForShadowMaster.AddListener(StopCheckForShadowMasterAchievement);
        EventService.Instance.CheckingTormentedSurvivorAchievement.AddListener(CheckForTomentedSurvivorAchievement);
    }


    public void ShowAchievement(AchievementTypes type)
    {
        StopAchievementCoroutine();
        switch (type)
        {
            case AchievementTypes.KeyMaster:
                achievementCoroutine = StartCoroutine(SetAchievement(keyMaster));
                break;
            case AchievementTypes.TormentedSurvivor:
                achievementCoroutine = StartCoroutine(SetAchievement(tormentedSurvivor));
                break;
            case AchievementTypes.MasterOfShadow:
                achievementCoroutine = StartCoroutine(SetAchievement(masterOfShadow));
                break;
            case AchievementTypes.SanitySaver:
                achievementCoroutine = StartCoroutine(SetAchievement(sanitySaver));
                break;
        }
    }

    private IEnumerator SetAchievement(AchievementSO achievement)
    {
        yield return new WaitForSeconds(achievement.WaitToTriggerDuration);
        ShowAchievementPopup(achievement);

        yield return new WaitForSeconds(achievement.DisplayDuration);
        HideAchievementPopup();
    }

    private void HideAchievementPopup()
    {
        achievementText.SetText(string.Empty);
        achievementPopup.SetActive(false);
        StopAchievementCoroutine();
    }

    private void ShowAchievementPopup(AchievementSO achievement)
    {
        achievementText.SetText(achievement.achievement);
        achievementPopup.SetActive(true);
    }

    public void StopAchievementCoroutine()
    {
        if (achievementCoroutine != null)
        {
            StopCoroutine(achievementCoroutine);
            achievementCoroutine = null;
        }
    }

    public void UpdateKeyValue(int key)
    {
        if (MaxNuberOfKeys == key)
        {
            ShowAchievement(AchievementTypes.KeyMaster);
            Debug.Log("key master.");
            EventService.Instance.OnAchievement.InvokeEvent();
        }
    }

    private void UpdateNumberOfPotionsConsumed(int val)
    {
        numberOfPotionsConsumed++;
        if (numberOfPotionsConsumed == MaxNumberOfPotion)
        {
            ShowAchievement(AchievementTypes.SanitySaver);
            Debug.Log("sanity saver");
            EventService.Instance.OnAchievement.InvokeEvent();
        }
    }

    private void CheckForTomentedSurvivorAchievement(float val)
    {
        if (val >= TormentedSurvivorLimit)
        {
            ShowAchievement(AchievementTypes.TormentedSurvivor);
            Debug.Log("tourmented survivor.");
            EventService.Instance.OnAchievement.InvokeEvent();
        }
    }

    private void CheckForShadowMasterAchievement()
    {
        shadowMasterCounter = 0;
        shadowMasterCoroutine = StartCoroutine(ShadowMasterCountDown());

    }

    private void StopCheckForShadowMasterAchievement()
    {
        if (shadowMasterCoroutine != null)
        {
            StopCoroutine(shadowMasterCoroutine);
            shadowMasterCoroutine = null;
        }
    }

    private IEnumerator ShadowMasterCountDown()
    {
        while (shadowMasterCounter < MasterOfShadowTimeLimit)
        {
            shadowMasterCounter += Time.deltaTime;
            //Debug.Log(shadowMasterCounter);
            yield return null;
        }

        if (shadowMasterCounter >= MasterOfShadowTimeLimit)
        {
            ShowAchievement(AchievementTypes.MasterOfShadow);
            Debug.Log("shadow master.");
            EventService.Instance.OnAchievement.InvokeEvent();
            shadowMasterCounter = 0;
        }
    }


    private void UnsubscribeToEvents()
    {
        EventService.Instance.OnKeyPickedUp.RemoveListener(UpdateKeyValue);
        EventService.Instance.OnPotionDrink.RemoveListener(UpdateNumberOfPotionsConsumed);
        EventService.Instance.StartCheckingForShadowMaster.RemoveListener(CheckForShadowMasterAchievement);
        EventService.Instance.StopCheckingForShadowMaster.RemoveListener(StopCheckForShadowMasterAchievement);
        EventService.Instance.CheckingTormentedSurvivorAchievement.RemoveListener(CheckForTomentedSurvivorAchievement);
    }

    private void OnDisable()
    {
        UnsubscribeToEvents();
        StopAllCoroutines();
    }
}
