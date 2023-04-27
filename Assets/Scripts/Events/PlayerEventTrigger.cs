using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO -> This Entire Script is not needed , Move this into PlayerView
public class PlayerEventTrigger : MonoBehaviour //view
{
    private bool Isinteracted; //naming

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Isinteracted = true;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            Isinteracted = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Todo -> Remove all these  Interactable class checks once each interable is refactored
        if (other.GetComponent<Interactable>() != null)
        {
            UIManager.Instance.ShowInteractInstructions(true);
        }

        if (other.GetComponent<I_Interactable>() != null)
        {
            UIManager.Instance.ShowInteractInstructions(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Interactable>() != null && Isinteracted)
        {
            Isinteracted = false;
            Debug.Log("Player Entered Interacted");
            other.GetComponent<Interactable>().Interact();
        }

        if (other.GetComponent<I_Interactable>() != null && Isinteracted)
        {
            Isinteracted = false;
            Debug.Log("Player Entered Interacted");
            other.GetComponent<I_Interactable>().Interact();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Interactable>() != null)
        {
            Debug.Log("Player Entered near Interactable");
            UIManager.Instance.ShowInteractInstructions(false);
        }
        if (other.GetComponent<I_Interactable>() != null)
        {
            Debug.Log("Player Entered near Interactable");
            UIManager.Instance.ShowInteractInstructions(false);
        }
    }
}
