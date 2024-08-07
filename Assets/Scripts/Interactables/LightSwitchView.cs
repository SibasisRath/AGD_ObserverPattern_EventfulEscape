using System.Collections.Generic;
using UnityEngine;

public class LightSwitchView : MonoBehaviour, IInteractable
{
    [SerializeField] private List<Light> lightsources = new();
    private SwitchState currentState;
    private bool test;

    public SwitchState CurrentState 
    { 
        get => currentState;
        private set => currentState = value;
    }

    private void OnEnable() 
    {
        EventService.Instance.OnLightSwitchToggled.AddListener(OnLightSwitch);
        EventService.Instance.OnLightsOffByGhostEvent.AddListener(OnLightSwitchOffByGhost);
    }

    private void OnDisable()
    {
        EventService.Instance.OnLightSwitchToggled.RemoveListener(OnLightSwitch);
        EventService.Instance.OnLightsOffByGhostEvent.RemoveListener(OnLightSwitchOffByGhost);
    }

    private void Start() => CurrentState = SwitchState.Off;


    public void Interact() => EventService.Instance.OnLightSwitchToggled.InvokeEvent();

    private void toggleLights()
    {
        bool lights = false;

        switch (CurrentState)
        {
            case SwitchState.On:
                CurrentState = SwitchState.Off;
                lights = false;
                break;
            case SwitchState.Off:
                CurrentState = SwitchState.On;
                lights = true;
                break;
            case SwitchState.Unresponsive:
                break;
        }
        foreach (Light lightSource in lightsources)
        {
            lightSource.enabled = lights;
        }
    }

    private void setLights(bool lights) 
    {
        foreach (Light lightSource in lightsources)
        {
            lightSource.enabled = lights;
        }
        if (lights)
        {
            CurrentState = SwitchState.On;
        }
        else
        {
            CurrentState = SwitchState.Off;
        }
    }

    private void OnLightSwitch()
    {
        toggleLights();
        GameService.Instance.GetSoundView().PlaySoundEffects(SoundType.SwitchSound);
        GameService.Instance.GetInstructionView().HideInstruction();
    }

    private void OnLightSwitchOffByGhost()
    {
        setLights(false);
        GameService.Instance.GetSoundView().PlaySoundEffects(SoundType.SwitchSound);
        GameService.Instance.GetInstructionView().HideInstruction();
    }
}
