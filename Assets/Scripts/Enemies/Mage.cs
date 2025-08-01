using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Lightning lightningPrefab;
    [SerializeField] private List<SpriteRenderer> spriteRenderers;
    private Hitbox hitbox;
    private TrackBody trackBody;

    [Header("Data")]
    [SerializeField] private float speedGain;
    [SerializeField] private float lightningChargeDuration = 10f;
    [SerializeField] private float targetPosMinX;
    [SerializeField] private float targetPosMaxX;

    [Header("Visual")]
    [SerializeField] private float gradientColorFrequency;
    [SerializeField] private Color gradientColor1;
    [SerializeField] private Color gradientColor2;
    [SerializeField] private float flashDuration;
    [SerializeField] private Color flashColor;
    [SerializeField] private Color castEndColor;

    private Lightning currentLightning;

    private float randomNum;
    private float targetPosX;

    private void Awake()
    {
        randomNum = Random.Range(0.0f, 99999.9f);

        hitbox = GetComponentInChildren<Hitbox>();
        hitbox.OnHit += OnHit;

        trackBody = GetComponent<TrackBody>();
    }

    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    private void Update()
    {
        if (currentLightning != null) currentLightning.SetBaseColor(GetGradientColor());
        MoveToPosX(targetPosX);
    }

    private void OnHit()
    {
        GameManager.Instance.EnemyKilled(speedGain);
        Destroy(gameObject);
    }

    private IEnumerator SpawnCoroutine()
    {
        float timer = 0.0f;
        targetPosX = Random.Range(targetPosMinX, targetPosMaxX);

        float distanceToTargetPos = Mathf.Abs(targetPosX - transform.position.x);
        while (distanceToTargetPos <= 4.0f)
        {
            timer += Time.deltaTime;
            SetColor(GetGradientColor());
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(CooldownCoroutine(initialChargePercent: Random.Range(0.3f, 0.5f)));
    }

    private IEnumerator CooldownCoroutine(float initialChargePercent = 0.0f)
    {
        float timer = initialChargePercent * lightningChargeDuration;

        while (timer < lightningChargeDuration)
        {
            timer += Time.deltaTime;
            SetColor(GetGradientColor());
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(CastingCoroutine());
    }

    private IEnumerator CastingCoroutine()
    {
        CastLightning();

        float timer = 0.0f;
        float timerNormalized;
        float strikeDuration = currentLightning.GetTimeToStrike();

        while (timer < strikeDuration)
        {
            timer += Time.deltaTime;
            timerNormalized = timer / strikeDuration;
            SetColor(Color.Lerp(GetGradientColor(), castEndColor, timerNormalized));
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(StrikeFlashCoroutine());
    }

    private IEnumerator StrikeFlashCoroutine()
    {
        float timer = 0.0f;
        float timerNormalized;

        SetColor(flashColor);

        while (timer < flashDuration)
        {
            timer += Time.deltaTime;
            timerNormalized = Mathf.Clamp(timer / flashDuration, 0.0f, 1.0f);
            SetColor(Color.Lerp(flashColor, GetGradientColor(), timerNormalized));
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(CooldownCoroutine());
    }
    

    private void CastLightning()
    {
        currentLightning = Instantiate(lightningPrefab);
    }

    private void MoveToPosX(float targetPosX)
    {
        if (transform.position.x > targetPosX)
        {
            trackBody.MoveHorizontal(-1.0f);
        }
        else
        {
            trackBody.MoveHorizontal(1.0f);
        }
    }

    public Color GetGradientColor()
    {
        float lerpVal = 0.5f * Mathf.Sin(Time.time * gradientColorFrequency + randomNum) + 0.5f;
        return Color.Lerp(gradientColor1, gradientColor2, lerpVal);
    }

    private void SetColor(Color newColor)
    {
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.color = newColor;
        }
    }
}
