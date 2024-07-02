using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWinEvent : MonoBehaviour
{
    [SerializeField] BoxCollider boxCollider;

    private void OnEnable()
    {
        boxCollider.enabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        EventService.Instance.PlayerEscapedEvent.InvokeEvent();
        boxCollider.enabled = false;
    }
}
