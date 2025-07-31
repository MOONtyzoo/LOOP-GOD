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


    private void Awake()
    {
        trackIdx = trackIdxMiddle;

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

        if (Input.GetButtonDown("MoveUp") && CanMoveUp())
        {
            MoveUp();
        }

        if (Input.GetButtonDown("MoveDown") && CanMoveDown())
        {
            MoveDown();
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
        bool shouldApplyTurnFactor = Mathf.Sign(inputDirection) != Mathf.Sign(rbody.linearVelocityX);
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
