using System;
using System.Collections;
using UnityEngine;
public class PlayerController : IDisposable
{
    private readonly PlayerView playerView;
    private readonly PlayerScriptableObject playerScriptableObject;
    private float velocity;
    private float horizontalAxis;
    private float verticalAxis;
    private float mouseX;
    private PlayerState playerState;

    public bool IsInteracted { get; set; }
    public int KeysEquipped { get => playerScriptableObject.KeysEquipped; set => playerScriptableObject.KeysEquipped = value; }
    public PlayerState PlayerState { get => playerState; private set => playerState = value; }

    public PlayerController(PlayerView playerView, PlayerScriptableObject playerScriptableObject)
    {
        this.playerView = playerView;
        this.playerView.SetController(this);
        this.playerScriptableObject = playerScriptableObject;
        this.playerScriptableObject.KeysEquipped = 0;
        PlayerState = PlayerState.InDark;

        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        EventService.Instance.OnLightsOffByGhostEvent.AddListener(OnLightsOffByGhost);
        EventService.Instance.OnLightSwitchToggled.AddListener(OnLightsToggled);
        EventService.Instance.OnKeyPickedUp.AddListener(OnKeyPickedUp);
        EventService.Instance.PlayerEscapedEvent.AddListener(OnPlayerEscaped);
    }

    private void UnsubscribeFromEvents()
    {
        EventService.Instance.OnLightsOffByGhostEvent.RemoveListener(OnLightsOffByGhost);
        EventService.Instance.OnLightSwitchToggled.RemoveListener(OnLightsToggled);
        EventService.Instance.OnKeyPickedUp.RemoveListener(OnKeyPickedUp);
        EventService.Instance.PlayerEscapedEvent.RemoveListener(OnPlayerEscaped);
    }

    public void Dispose()
    {
        UnsubscribeFromEvents();
        EventService.Instance.StopCheckingForShadowMaster.InvokeEvent();
    }

    public void Interact() => IsInteracted = Input.GetKeyDown(KeyCode.E) || (!Input.GetKeyUp(KeyCode.E) && IsInteracted);

    public void Jump(Rigidbody playerRigidbody, Transform transform)
    {
        bool IsGrounded = Physics.Raycast(transform.position, -transform.up, playerScriptableObject.raycastLength);

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            playerRigidbody.AddForce(Vector3.up * playerScriptableObject.jumpForce, ForceMode.Impulse);
        }
    }

    public void Move(Rigidbody playerRigidbody, Transform transform)
    {
        GetInput();

        CalculatePositionRotation(playerRigidbody, transform, out Quaternion rotation, out Vector3 position);

        playerRigidbody.MoveRotation(rotation);
        playerRigidbody.MovePosition(position);
    }

    public void KillPlayer()
    {
        EventService.Instance.StopCheckingForShadowMaster.InvokeEvent();
        PlayerState = PlayerState.Dead;
        DisableControls();
    }

    private void OnPlayerEscaped()
    {
        Debug.Log("inside player controller.");
        EventService.Instance.StopCheckingForShadowMaster.InvokeEvent();
        PlayerState = PlayerState.Escaped;
        //DisableControls();
    }

    private void OnLightsOffByGhost()
    {
        PlayerState = PlayerState.InDark;
        EventService.Instance.StartCheckingForShadowMaster.InvokeEvent();
    }
    private void OnKeyPickedUp(int keys) => KeysEquipped = keys;
    private void DisableControls() => playerView.enabled = false;

    private void GetInput()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");
        mouseX = Input.GetAxis("Mouse X");
        velocity = Input.GetKey(KeyCode.LeftShift) ? playerScriptableObject.sprintSpeed : playerScriptableObject.walkSpeed;
    }
    private void CalculatePositionRotation(Rigidbody playerRigidbody, Transform transform, out Quaternion rotation, out Vector3 position)
    {
        Vector3 lookRotation = new(0, mouseX * playerScriptableObject.sensitivity, 0);
        Vector3 movement = (transform.forward * verticalAxis + transform.right * horizontalAxis);

        rotation = playerRigidbody.rotation * Quaternion.Euler(lookRotation);
        position = (transform.position) + (velocity * movement) * Time.fixedDeltaTime;
    }
    private void OnLightsToggled()
    {
        if (PlayerState == PlayerState.InDark)
        {
            PlayerState = PlayerState.None;
            EventService.Instance.StopCheckingForShadowMaster.InvokeEvent();
        }
        else 
        {
            PlayerState = PlayerState.InDark;
            EventService.Instance.StartCheckingForShadowMaster.InvokeEvent();
        }
            
    }
}
