using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Horizontal Movement")]
    [SerializeField] private float forwardSpeedMax;
    [SerializeField] private float backwardSpeedMax;
    [SerializeField] private float acceleration;
    [SerializeField] private float turnFactor;

    private Rigidbody2D rbody;

    private int trackNum = 2;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        MoveHorizontal(horizontalInput);

        if (Input.GetButtonDown("MoveUp"))
        {
            Debug.Log("Move up!");
        }

        if (Input.GetButtonDown("MoveDown"))
        {
            Debug.Log("Move down!");
        }

        if (Input.GetButtonDown("Sword"))
        {
            Debug.Log("Sword!");
        }

        if (Input.GetButtonDown("Gun"))
        {
            Debug.Log("Gun!");
        }

        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump!");
        }
    }

    private void MoveHorizontal(float inputDirection)
    {
        float targetVelocityX = 0;
        if (inputDirection == 0) {
            targetVelocityX = 0;
        } else if (inputDirection > 0) {
            targetVelocityX = forwardSpeedMax;
        } else if (inputDirection < 0) {
            targetVelocityX = -backwardSpeedMax;
        }

        float maxDelta = acceleration * Time.deltaTime;
        bool shouldApplyTurnFactor = Mathf.Sign(inputDirection) != Mathf.Sign(rbody.linearVelocityX);
        if (shouldApplyTurnFactor) maxDelta *= turnFactor;

        rbody.linearVelocityX = Mathf.MoveTowards(rbody.linearVelocityX, targetVelocityX, maxDelta);
    }
}
