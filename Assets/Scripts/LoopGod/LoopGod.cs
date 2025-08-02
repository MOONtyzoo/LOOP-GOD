using System;
using System.Collections.Generic;
using UnityEngine;

public class LoopGod : MonoBehaviour
{
    public event Action OnHitPlayer;

    [Header("References")]
    [SerializeField, Tooltip("The x position at which the loop god is considered to be at \"zero\" distance.")]
    private float distanceZeroPosX;
    [SerializeField, Tooltip("Distance at which the loop god becomes visible on screen.")]
    private float visibleDistance;
    [SerializeField] private Player player;
    private Hurtbox hurtbox;

    [Header("Gameplay Tuning")]
    [SerializeField, Tooltip("The furthest possible distance the loop god can be. Even if the player is moving faster, the loop god will not fall back further than this point.")]
    private float followDistanceLimit;
    [SerializeField, Tooltip("The speed at which the loop god follows the player. If this is greater than the player's speed, the loop god will gain distance.")]
    private float followSpeed;
    [SerializeField]
    private float distanceFactor;

    [Header("Head Visual")]
    [SerializeField] private Animator headAnimator;
    [SerializeField] private Transform headTransform;
    [SerializeField] private float weaveAmplitude;
    [SerializeField] private float weaveFrequency;
    [SerializeField] private float mouthOpenDistance;

    [Header("Body Visual")]
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private Transform segmentPrefab;
    [SerializeField] private int segmentCount;
    [SerializeField] private float segmentSeparation;
    [SerializeField] private float segmentWeaveOffset;
    private List<Transform> segments;

    private float currentDistance;
    private float mouthOpenAmount;

    private void Awake()
    {
        hurtbox = GetComponentInChildren<Hurtbox>();
        hurtbox.OnHit += Hurtbox_OnHit;

        segments = new List<Transform>();
        for (int i = 0; i < segmentCount; i++)
        {
            Transform segment = Instantiate(segmentPrefab, bodyTransform);
            segments.Add(segment);
        }
    }

    private void Start()
    {
        SetDistance(followDistanceLimit);
    }

    private void Hurtbox_OnHit()
    {
        OnHitPlayer?.Invoke();
    }

    private void Update()
    {
        FollowPlayer();
        AnimateSnake();
    }

    private void FollowPlayer()
    {
        float relativeSpeed = followSpeed - GameManager.Instance.GetPlayerSpeed();
        SetDistance(currentDistance - relativeSpeed * Time.deltaTime);
    }

    private void AnimateSnake()
    {
        float snakePosX = distanceZeroPosX - currentDistance * distanceFactor;
        transform.position = new Vector2(snakePosX, transform.position.y);

        float headPosY = GetWeavePosY(0);
        headTransform.position = new Vector2(headTransform.position.x, headPosY);

        float distanceFromPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);
        float targetMouthOpenAmount = (distanceFromPlayer < mouthOpenDistance) ? 1.0f : 0.0f;
        mouthOpenAmount = Mathf.Lerp(mouthOpenAmount, targetMouthOpenAmount, 4.0f * Time.deltaTime);
        headAnimator.SetFloat("MouthOpen", mouthOpenAmount);

        AnimateBodySegments();
    }

    private void AnimateBodySegments()
    {
        for (int i = 0; i < segmentCount; i++)
        {
            Transform segment = segments[i];
            float segmentPosX = -segmentSeparation * i;
            float segmentPosY = GetWeavePosY(i + 1);
            segment.transform.localPosition = new Vector2(segmentPosX, segmentPosY);
        }
    }

    private float GetWeavePosY(int segmentNum)
    {
        return weaveAmplitude * Mathf.Sin(weaveFrequency * (Time.time + segmentNum * segmentWeaveOffset));
    }

    private void SetDistance(float newDistance)
    {
        currentDistance = Mathf.Clamp(newDistance, 0.0f, followDistanceLimit);
    }

    public float GetDistance() => currentDistance;
}
