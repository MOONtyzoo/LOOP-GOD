using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rbody;

    [Header("Horizontal Movement")]
    [SerializeField] private float forwardSpeedMax;
    [SerializeField] private float backwardSpeedMax;
    [SerializeField] private float acceleration;
    [SerializeField] private float turnFactor;

    [Header("Track Data")]
    [SerializeField] private float trackDistance;
    [SerializeField] private int numTracks;
    [SerializeField] private int trackIdxTop;
    [SerializeField] private int trackIdxBottom;
    [SerializeField] private int trackIdxMiddle;
    private int trackIdx;

    [Header("Vertical Movement")]
    [SerializeField] private float trackSwitchDuration;
    private bool isChangingTracks = false;

    private float horizontalInput;
    private InputButton MoveUpButton;
    private InputButton MoveDownButton;
    private InputButton SwordButton;
    private InputButton GunButton;
    private InputButton JumpButton;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();

        trackIdx = trackIdxMiddle;

        MoveUpButton = new InputButton("MoveUp", 0.5f*trackSwitchDuration);
        MoveDownButton = new InputButton("MoveDown", 0.5f*trackSwitchDuration);
        SwordButton = new InputButton("Sword", 0.0f);
        GunButton = new InputButton("Gun", 0.0f);
        JumpButton = new InputButton("Jump", 0.0f);
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        MoveHorizontal(horizontalInput);
    }

    private void HandleInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        UpdateInputButtons();

        if (MoveUpButton.WasPressed() && CanMoveUp())
        {
            MoveUp();
        }

        if (MoveDownButton.WasPressed() && CanMoveDown())
        {
            MoveDown();
        }

        if (SwordButton.WasPressed())
        {
            Debug.Log("Sword!");
        }

        if (GunButton.WasPressed())
        {
            Debug.Log("Gun!");
        }

        if (JumpButton.WasPressed())
        {
            Debug.Log("Jump!");
        }
    }

    private void UpdateInputButtons()
    {
        MoveUpButton.Update();
        MoveDownButton.Update();
        SwordButton.Update();
        GunButton.Update();
        JumpButton.Update();
    }

    private void MoveHorizontal(float inputDirection)
    {
        float targetVelocityX = 0;
        if (inputDirection == 0)
        {
            targetVelocityX = 0;
        }
        else if (inputDirection > 0)
        {
            targetVelocityX = forwardSpeedMax;
        }
        else if (inputDirection < 0)
        {
            targetVelocityX = -backwardSpeedMax;
        }

        float maxDelta = acceleration * Time.deltaTime;
        bool shouldApplyTurnFactor = inputDirection == 0.0f || Mathf.Sign(inputDirection) != Mathf.Sign(rbody.linearVelocityX);
        if (shouldApplyTurnFactor) maxDelta *= turnFactor;

        rbody.linearVelocityX = Mathf.MoveTowards(rbody.linearVelocityX, targetVelocityX, maxDelta);
    }


    private bool CanMoveUp() => trackIdx != trackIdxTop && !isChangingTracks;
    private void MoveUp() {
        StartCoroutine(changeTrackCoroutine(transform.position.y + trackDistance, trackIdx-1));
    }

    private bool CanMoveDown() => trackIdx != trackIdxBottom && !isChangingTracks;
    private void MoveDown()
    {
        StartCoroutine(changeTrackCoroutine(transform.position.y - trackDistance, trackIdx+1));
    }

    private IEnumerator changeTrackCoroutine(float targetPosY, int targetTrackIdx)
    {
        float duration = trackSwitchDuration;
        float timer = 0;
        float timerNormalized;

        float startPosY = transform.position.y;

        isChangingTracks = true;
        trackIdx = targetTrackIdx;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            timerNormalized = timer / duration;

            float cubicLerpVal = 1 + Mathf.Pow(timerNormalized - 1, 3f);
            float lerpedPosY = Mathf.Lerp(startPosY, targetPosY, cubicLerpVal);
            rbody.position = new Vector2(rbody.position.x, lerpedPosY);
            yield return new WaitForEndOfFrame();
        }
        isChangingTracks = false;
    }
}
