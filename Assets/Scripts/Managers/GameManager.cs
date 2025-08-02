using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action OnEnemyKilled;

    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private LoopGod loopGod;

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

    private float distance = 0.0f;
    private float speed = 0.0f;
    private bool isRunAccelerationStunned = false;
    private bool isEquilibriumDecayStunned = false;

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

    private void Player_OnHit()
    {
        StartCoroutine(StunAccelerationCoroutine());

        float reductionFactor = Mathf.Max(1f, hitReductionMultiplier * (speed - naturalRunSpeed));
        DecreaseSpeed(hitReductionMin*reductionFactor);
    }

    private void LoopGod_OnHitPlayer()
    {
        EndGame();
    }

    private void Update()
    {
        UpdateSpeed();
        distance += speed * Time.deltaTime;
    }

    private void UpdateSpeed()
    {
        if (speed < naturalRunSpeed && !isRunAccelerationStunned)
        {
            speed += runAcceleration * Time.deltaTime;
        }

        if (speed > equilibriumSpeed && !isEquilibriumDecayStunned)
        {
            float decelerationScaling = Mathf.Pow(speed-equilibriumSpeed, equilibriumDecayExponent);
            speed -= equilibirumDecayRate * decelerationScaling * Time.deltaTime;
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

    public float GetDistance() => distance;
    public float GetPlayerSpeed() => speed;

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

    public void SetSpeed(float newSpeed)
    {
        speed = Mathf.Clamp(newSpeed, 0.0f, terminalSpeed);
    }

    public Vector2 GetPlayerPosition() => player.transform.position;
}
