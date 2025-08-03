using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action OnEnemyKilled;
    public event Action<float> OnSpeedChanged;
    public event Action<int> OnLapChanged;

    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private LoopGod loopGod;
    [SerializeField] private TrackScroller trackScroller;
    [SerializeField] private EnemySpawner enemySpawner;

    [Header("Speed Adjustments")]
    [SerializeField, Tooltip("If lower than this speed, naturally accelerate to it.")]
    private float naturalRunSpeed;
    [SerializeField, Tooltip("If higher than this speed, naturally decelerate to it.")]
    private float equilibriumSpeed;
    [SerializeField, Tooltip("Can not go faster than this speed no matter what")]
    private float terminalSpeed;

    [Space]
    [SerializeField, Tooltip("Acceleration to natural speed from a lower speed")]
    private float runAcceleration;
    [SerializeField, Tooltip("Acceleration is paused for this long after taking a hit")]
    private float accelerationStunDuration;

    [Space]
    [SerializeField, Tooltip("Decay to equilibrium speed from a higher speed")]
    private float equilibirumDecayRate;
    [SerializeField, Tooltip("Exponent by which deceleration is proprtional to speed. 0 means that all speeds have the same deceleration, while 1 means that deceleration is proprtional to speed.")]
    private float equilibriumDecayExponent;
    [SerializeField, Tooltip("Decay is paused for this long after gaining speed from an enemy")]
    private float equilibriumDecayStunDuration;

    [Space]
    [SerializeField, Tooltip("Minimum guarunteed speed loss on taking damage.")]
    private float hitReductionMin;
    [SerializeField, Tooltip("Multiplier for how much speed is lost on hit proprtional to speed above natural run speed.")]
    private float hitReductionMultiplier;

    [Header("Difficulty Scaling")]
    [SerializeField] private List<DifficultyRange> difficultyRanges = new List<DifficultyRange>();
    private DifficultyLevel currentDifficultyLevel;

    [Serializable]
    private struct DifficultyRange
    {
        public int lapRangeMin;
        public int lapRangeMax; 
        public DifficultyLevel difficultyLevel;
    }

    private float distance = 0.0f;
    private float lapProgress = 0.0f;
    private float speed = 0.0f;
    private int currentLap;
    private bool isRunAccelerationStunned = false;
    private bool isEquilibriumDecayStunned = false;
    private bool lapProgressPaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Error attempting to instantiate singleton \"GameManager\", There is already one in the scene!");
        }

        player.OnHit += Player_OnHit;
        loopGod.OnHitPlayer += LoopGod_OnHitPlayer;
    }

    private void Start()
    {
        SetLap(1);
    }

    private void Player_OnHit()
    {
        StartCoroutine(StunAccelerationCoroutine());

        float reductionFactor = Mathf.Max(1f, hitReductionMultiplier * (speed - naturalRunSpeed));
        DecreaseSpeed(hitReductionMin * reductionFactor);
    }

    private void LoopGod_OnHitPlayer()
    {
        EndGame();
    }

    private void Update()
    {
        UpdateSpeed();
        if (!lapProgressPaused)
        {
            SetLapProgress(lapProgress + Time.deltaTime * speed / GetLapLength());
        }
        distance += Time.deltaTime * speed;
    }

    private void UpdateSpeed()
    {
        if (speed < naturalRunSpeed && !isRunAccelerationStunned)
        {
            IncreaseSpeed(runAcceleration * Time.deltaTime);
        }

        if (speed > equilibriumSpeed && !isEquilibriumDecayStunned)
        {
            float decelerationScaling = Mathf.Pow(speed-equilibriumSpeed, equilibriumDecayExponent);
            DecreaseSpeed(equilibirumDecayRate * decelerationScaling * Time.deltaTime);
        }
    }

    private IEnumerator StunAccelerationCoroutine()
    {
        isRunAccelerationStunned = true;

        float timer = 0.0f;
        while (timer < accelerationStunDuration)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        isRunAccelerationStunned = false;
    }

    private IEnumerator StunEquilibriumDecayCoroutine()
    {
        isEquilibriumDecayStunned = true;

        float timer = 0.0f;
        while (timer < equilibriumDecayStunDuration)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        isEquilibriumDecayStunned = false;
    }

    public void SetLapProgress(float newLapProgress)
    {
        lapProgress = newLapProgress;
        if (lapProgress > 1.0f) SetLap(currentLap + 1);
    }

    public int GetLap() => currentLap;
    public void SetLap(int newLap)
    {
        currentLap = newLap;
        lapProgress = 0.0f;
        OnLapChanged?.Invoke(newLap);

        if (GetDifficultyLevelForLap(newLap) != currentDifficultyLevel)
        {
            SetDifficultyLevel(GetDifficultyLevelForLap(newLap));
        }
    }
    public float GetLapProgress() => lapProgress;
    public float GetDistance() => distance;

    public DifficultyLevel GetDifficultyLevel() => currentDifficultyLevel;
    public void SetDifficultyLevel(DifficultyLevel newDifficultyLevel)
    {
        currentDifficultyLevel = newDifficultyLevel;
        loopGod.SetFollowSpeed(currentDifficultyLevel.loopGodFollowSpeed);
        lapProgressPaused = newDifficultyLevel.pausesLapProgress;
        trackScroller.SetTrackPrefabs(newDifficultyLevel.trackPrefabs);
        enemySpawner.SetEnemySpawnDataList(newDifficultyLevel.enemySpawnData);
    }
    DifficultyLevel GetDifficultyLevelForLap(int lap)
    {
        foreach (DifficultyRange difficultyRange in difficultyRanges)
        {
            if (lap >= difficultyRange.lapRangeMin && lap <= difficultyRange.lapRangeMax)
            {
                return difficultyRange.difficultyLevel;
            }
        }

        Debug.LogError("Could not map a lap "+ lap.ToString() + " to a difficulty level!");
        return null;
    }

    public void EnemyKilled(float speedGain)
    {
        IncreaseSpeed(speedGain);
        StartCoroutine(StunEquilibriumDecayCoroutine());
        OnEnemyKilled?.Invoke();
    }

    public void EndGame()
    {
        Debug.Log("Game Over!");
    }

    public void IncreaseSpeed(float amount)
    {
        SetSpeed(speed + amount);
    }

    public void DecreaseSpeed(float amount)
    {
        SetSpeed(speed - amount);
    }

    public float GetSpeed() => speed;
    public void SetSpeed(float newSpeed)
    {
        float oldSpeed = speed;
        speed = Mathf.Clamp(newSpeed, 0.0f, terminalSpeed);
        if (speed != oldSpeed) OnSpeedChanged?.Invoke(speed);
    }

    public float GetLapLength() => currentDifficultyLevel.lapLength;

    public float GetTerminalSpeed() => terminalSpeed;
    public Vector2 GetPlayerPosition() => player.transform.position;
}
