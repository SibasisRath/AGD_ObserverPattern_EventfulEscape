using UnityEngine;

public class GameWinEvent : MonoBehaviour
{
    [SerializeField] BoxCollider boxCollider;

    private void OnEnable()
    {
        boxCollider.enabled = true;
        Debug.Log("collider enabled.");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerView>() != null)
        {
            EventService.Instance.PlayerEscapedEvent.InvokeEvent();
            Debug.Log("game win trigger");
            boxCollider.enabled = false;
        }
    }
}
