using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TrackBody : MonoBehaviour
{
    [Header("Horizontal Movement")]
    [SerializeField] private float forwardSpeedMax;
    [SerializeField] private float backwardSpeedMax;
    [SerializeField] private float acceleration;
    [SerializeField] private float turnFactor;

    [Header("Track Movement")]
    [SerializeField] private float trackSwitchDuration;

    private const float TRACK_DISTANCE = 2.0f;
    private const int TRACK_IDX_TOP = 0;
    private const int TRACK_IDX_MIDDLE = 2;
    private const int TRACK_IDX_BOTTOM = 4;
    private const int NUM_TRACKS = 5;

    private bool isChangingTracks = false;
    private int trackIdx = 2;

    private Rigidbody2D rbody;

    private void Awake()
    {
        rbody = GetComponentInParent<Rigidbody2D>();
    }

    public void MoveToRandomTrack() => MoveToTrack(Random.Range(TRACK_IDX_TOP, TRACK_IDX_BOTTOM + 1));
    public void MoveToAdjacentTrack()
    {
        if (!CanMoveDown()) MoveUp();
        if (!CanMoveUp()) MoveDown();
        if (Random.Range(0, 2) == 0)
        {
            MoveUp();
        } else
        {
            MoveDown();
        }
    }
    public void MoveToTrack(int targetTrackIdx) => StartCoroutine(changeTrackCoroutine(targetTrackIdx));

    public bool CanMoveUp() => trackIdx != TRACK_IDX_TOP && !isChangingTracks;
    public void MoveUp()
    {
        StartCoroutine(changeTrackCoroutine(trackIdx - 1));
    }

    public bool CanMoveDown() => trackIdx != TRACK_IDX_BOTTOM && !isChangingTracks;
    public void MoveDown()
    {
        StartCoroutine(changeTrackCoroutine(trackIdx + 1));
    }

    private IEnumerator changeTrackCoroutine(int targetTrackIdx)
    {
        float duration = trackSwitchDuration;
        float timer = 0;
        float timerNormalized;

        float startPosY = transform.position.y;
        targetTrackIdx = Mathf.Clamp(targetTrackIdx, TRACK_IDX_TOP, TRACK_IDX_BOTTOM);
        float targetPosY = GetTrackPosY(targetTrackIdx);

        isChangingTracks = true;
        SetTrack(targetTrackIdx);
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

    public void MoveHorizontal(float inputDirection)
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

    public void SetTrack(int newTrackIdx)
    {
        trackIdx = Mathf.Clamp(newTrackIdx, TRACK_IDX_TOP, TRACK_IDX_BOTTOM);
        rbody.position = new Vector2(rbody.position.x, GetTrackPosY(trackIdx));
    }

    private float GetTrackPosY(int trackIdx)
    {
        float trackPosY = TRACK_DISTANCE * -(trackIdx - (NUM_TRACKS - 1) / 2);
        return trackPosY;
    }

    public int GetTrack() => trackIdx;
    public float GetTrackSwitchDuration() => trackSwitchDuration;
}
