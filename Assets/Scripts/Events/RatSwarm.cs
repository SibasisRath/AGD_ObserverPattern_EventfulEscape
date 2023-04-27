using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO ->Remove this script-> Move this entire logic in RatRushEvent.cs
public class RatSwarm : MonoBehaviour
{
    [SerializeField] private Transform Rats;
    [SerializeField] private Transform target;
    private float speed = 7.5f;

    private bool rushActive = false;
    private bool reachedTarget = false;
    private Coroutine RatRushCorroutine;


    private void OnEnable()
    {
        EventManager.OnRatRush += OnRatRush;
    }

    private void OnDisable()
    {
        EventManager.OnRatRush -= OnRatRush;
    }

    void Update()
    {
        if (rushActive)
        {
            if (!reachedTarget)
            {
                Rats.position = Vector3.MoveTowards(Rats.position, target.position, speed * Time.deltaTime);

                if (Rats.position == target.position)
                {
                    reachedTarget = true;
                }
            }
            else
            {
                rushActive = false;
                Rats.gameObject.SetActive(false);
            }
        }
    }

    private void OnRatRush()
    {
        Debug.Log("OnRatRush Event Occurred.");
        Rats.gameObject.SetActive(true);
        rushActive = true;
    }

}
