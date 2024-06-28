using UnityEngine;

public class KeyView : MonoBehaviour, IInteractable
{
    [SerializeField] GameUIView gameUIView;
    public void Interact()
    {
        int currentKeys = GameService.Instance.GetPlayerController().KeysEquipped;
        
        GameService.Instance.GetInstructionView().HideInstruction();
        GameService.Instance.GetSoundView().PlaySoundEffects(SoundType.KeyPickUp);

        currentKeys++;
        EventService.Instance.OnKeyPickUp.InvokeEvent(currentKeys);

        gameObject.SetActive(false);
    }
}
