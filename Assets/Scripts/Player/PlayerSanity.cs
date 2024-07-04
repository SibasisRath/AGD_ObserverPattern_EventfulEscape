using UnityEngine;

public class PlayerSanity : MonoBehaviour
{
    [SerializeField] private float sanityLevel = 100.0f;
    [SerializeField] private float sanityDropRate = 0.2f;
    [SerializeField] private float sanityDropAmountPerEvent = 10f;

    private float maxSanity;
    private PlayerController playerController;


    private void OnEnable()
    {
        EventService.Instance.OnRatRush.AddListener(OnSupernaturalEvent);
        EventService.Instance.OnSkullDrop.AddListener(OnSupernaturalEvent);
        EventService.Instance.OnPotionDrink.AddListener(OnDrankPotion);
        EventService.Instance.PlayerEscapedEvent.AddListener(PlayerSanityLeft);
    }

    private void OnDisable()
    {
        EventService.Instance.OnRatRush.RemoveListener(OnSupernaturalEvent);
        EventService.Instance.OnSkullDrop.RemoveListener(OnSupernaturalEvent);
        EventService.Instance.OnPotionDrink.RemoveListener(OnDrankPotion);
        EventService.Instance.PlayerEscapedEvent.RemoveListener(PlayerSanityLeft);
    }

    private void Start()
    {
        maxSanity = sanityLevel;
        playerController = GameService.Instance.GetPlayerController();
    }
    void Update()
    {
        if (playerController.PlayerState == PlayerState.Dead || playerController.PlayerState == PlayerState.Escaped)
            return;

        float sanityDrop = UpdateSanity();

        IncreaseSanity(sanityDrop);
    }

    private float UpdateSanity()
    {
        float sanityDrop = sanityDropRate * Time.deltaTime;
        if (playerController.PlayerState == PlayerState.InDark)
        {
            sanityDrop *= 10f;
        }
        return sanityDrop;
    }

    private void IncreaseSanity(float amountToDecrease)
    {
        Mathf.Floor(sanityLevel -= amountToDecrease);
        if (sanityLevel <= 0)
        {
            sanityLevel = 0;
            GameService.Instance.GameOver();
        }
        GameService.Instance.GetGameUI().UpdateInsanity(1f - sanityLevel / maxSanity);
    }

    private void DecreaseSanity(float amountToIncrease)
    {
        Mathf.Floor(sanityLevel += amountToIncrease);
        if (sanityLevel > 100)
        {
            sanityLevel = 100;
        }
        GameService.Instance.GetGameUI().UpdateInsanity(1f - sanityLevel / maxSanity);
    }
    private void OnSupernaturalEvent()
    {
        IncreaseSanity(sanityDropAmountPerEvent);
    }

    private void OnDrankPotion(int potionEffect)
    {
        DecreaseSanity(potionEffect);
    }

    private void PlayerSanityLeft()
    {
        float leftSanityPercent = (sanityLevel / maxSanity) * 100;
        EventService.Instance.CheckingTormentedSurvivorAchievement.InvokeEvent(leftSanityPercent);
        Debug.Log("player last sanity " + leftSanityPercent);
    }
}