using UnityEngine;

public class PlayerController
{
    public bool IsInteracted;

    private readonly PlayerView playerView;
    private readonly PlayerScriptableObject playerScriptableObject;
    private float velocity;
    private float horizontalAxis;
    private float verticalAxis;
    private float mouseX;
    private PlayerState playerState;

    public int KeysEquipped { get => playerScriptableObject.KeysEquipped; set => playerScriptableObject.KeysEquipped = value; }
    public PlayerState PlayerState { get => playerState; private set => playerState = value; }


    public PlayerController(PlayerView playerView, PlayerScriptableObject playerScriptableObject)
    {
        this.playerView = playerView;
        this.playerView.SetController(this);
        this.playerScriptableObject = playerScriptableObject;
        this.playerScriptableObject.KeysEquipped = 0;
        playerState = PlayerState.InDark;

        EventService.Instance.OnLightSwitchToggled.AddListener(onLightSwitch);
        EventService.Instance.OnLightsOffByGhostEvent.AddListener(onLightsTurnedOffByGhost);
        EventService.Instance.OnKeyPickedUp.AddListener(onKeysPickedUp);
    }

    ~PlayerController()
    {
        EventService.Instance.OnLightSwitchToggled.RemoveListener(onLightSwitch);
        EventService.Instance.OnLightsOffByGhostEvent.RemoveListener(onLightsTurnedOffByGhost);
        EventService.Instance.OnKeyPickedUp.RemoveListener(onKeysPickedUp);
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

        calculatePositionRotation(playerRigidbody, transform, out Quaternion rotation, out Vector3 position);

        playerRigidbody.MoveRotation(rotation);
        playerRigidbody.MovePosition(position);
    }

    public void KillPlayer()
    {
        PlayerState = PlayerState.Dead;
    }

    private void GetInput()
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

    private void onLightSwitch()
    {
        if (PlayerState == PlayerState.InDark)
            PlayerState = PlayerState.None;
        else
            PlayerState = PlayerState.InDark;
    }

    private void onLightsTurnedOffByGhost() => PlayerState = PlayerState.InDark;

    private void onKeysPickedUp(int keys)
    {
        KeysEquipped = keys;
    }
}
