using UnityEngine;
public class PlayerController
{
    private PlayerView playerView;
    private PlayerScriptableObject playerScriptableObject;
    private float velocity;
    private float horizontalAxis;
    private float verticalAxis;
    private float mouseX;
    private PlayerState playerState;

    public bool IsInteracted;
    public int KeysEquipped { get => playerScriptableObject.KeysEquipped; set => playerScriptableObject.KeysEquipped = value; }
    public PlayerState PlayerState { get => playerState; private set => playerState = value; }

    public PlayerController(PlayerView playerView, PlayerScriptableObject playerScriptableObject)
    {
        this.playerView = playerView;
        this.playerView.SetController(this);
        this.playerScriptableObject = playerScriptableObject;
        this.playerScriptableObject.KeysEquipped = 0;
        playerState = PlayerState.InDark;

        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        EventService.Instance.OnLightsOffByGhostEvent.AddListener(OnLightsOffByGhost);
        EventService.Instance.OnLightSwitchToggled.AddListener(onLightsToggled);
        EventService.Instance.OnKeyPickedUp.AddListener(OnKeyPickedUp);
        EventService.Instance.PlayerEscapedEvent.AddListener(OnPlayerEscaped);
    }

    private void UnsubscribeFromEvents()
    {
        EventService.Instance.OnLightsOffByGhostEvent.RemoveListener(OnLightsOffByGhost);
        EventService.Instance.OnLightSwitchToggled.RemoveListener(onLightsToggled);
        EventService.Instance.OnKeyPickedUp.RemoveListener(OnKeyPickedUp);
        EventService.Instance.PlayerEscapedEvent.RemoveListener(OnPlayerEscaped);
    }

    ~PlayerController()
    {
        UnsubscribeFromEvents();
    }
    public void Interact() => IsInteracted = Input.GetKeyDown(KeyCode.E) ? true : (Input.GetKeyUp(KeyCode.E) ? false : IsInteracted);

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
        getInput();

        calculatePositionRotation(playerRigidbody, transform, out Quaternion rotation, out Vector3 position);

        playerRigidbody.MoveRotation(rotation);
        playerRigidbody.MovePosition(position);
    }

    public void KillPlayer()
    {
        PlayerState = PlayerState.Dead;
        DisableControls();
        UnsubscribeFromEvents();
    }

    private void OnLightsOffByGhost() => PlayerState = PlayerState.InDark;
    private void OnKeyPickedUp(int keys) => KeysEquipped = keys;
    private void DisableControls() => playerView.enabled = false;

    private void getInput()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");
        mouseX = Input.GetAxis("Mouse X");
        velocity = Input.GetKey(KeyCode.LeftShift) ? playerScriptableObject.sprintSpeed : playerScriptableObject.walkSpeed;
    }
    private void calculatePositionRotation(Rigidbody playerRigidbody, Transform transform, out Quaternion rotation, out Vector3 position)
    {
        Vector3 lookRotation = new(0, mouseX * playerScriptableObject.sensitivity, 0);
        Vector3 movement = (transform.forward * verticalAxis + transform.right * horizontalAxis);

        rotation = playerRigidbody.rotation * Quaternion.Euler(lookRotation);
        position = (transform.position) + (velocity * movement) * Time.fixedDeltaTime;
    }
    private void onLightsToggled()
    {
        if (PlayerState == PlayerState.InDark)
            PlayerState = PlayerState.None;
        else
            PlayerState = PlayerState.InDark;
    }

    private void OnPlayerEscaped()
    {
        DisableControls();
        UnsubscribeFromEvents();
    }
}
