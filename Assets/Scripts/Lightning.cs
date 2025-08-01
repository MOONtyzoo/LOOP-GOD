using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer outlineSprite;
    [SerializeField] private SpriteRenderer lightningSprite;
    [SerializeField] private TextMeshPro warningLabel;
    [SerializeField] private Hurtbox hurtbox;
    private Mage caster;

    [Header("Stats")]
    [SerializeField] private float followDuration;
    [SerializeField] private float warningDuration;
    [SerializeField] private float strikeDuration;
    [SerializeField] private float followAccel;
    [SerializeField] private float followSpeedMax;

    [Header("Visual")]
    [SerializeField] private float fadeInDuration;
    [SerializeField] private Color transparentColor;
    [SerializeField] private float warningFlashTime;
    [SerializeField] private Color warningFlashColor;

    private Color baseColor;

    private void Start()
    {
        SetColor(transparentColor);
        hurtbox.Disable();

        SetPositionToPlayer();
        StartCoroutine(FollowCoroutine());
    }

    private IEnumerator FollowCoroutine()
    {
        float timer = 0.0f;
        float fadeTimerNormalized;

        while (timer < followDuration)
        {
            timer += Time.deltaTime;
            fadeTimerNormalized = Mathf.Clamp(timer / fadeInDuration, 0.0f, 1.0f);
            SetColor(Color.Lerp(transparentColor, baseColor, fadeTimerNormalized));
            FollowPlayer();
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(WarningCoroutine());
    }

    private IEnumerator WarningCoroutine()
    {
        float timer = 0.0f;
        float warningFlashTimer = 0.0f;
        float warningFlashIdx = 0;

        while (timer < warningDuration)
        {
            timer += Time.deltaTime;
            warningFlashTimer += Time.deltaTime;
            if (warningFlashTimer > warningFlashTime * 0.5f)
            {
                warningFlashIdx = (warningFlashIdx + 1) % 2;
                warningFlashTimer -= warningFlashTime * 0.5f;

            }

            if (warningFlashIdx == 0)
            {
                SetColor(baseColor);
            }
            else
            {
                SetColor(warningFlashColor);
            }
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(StrikeCoroutine());
    }

    private IEnumerator StrikeCoroutine()
    {
        float timer = 0.0f;
        float timerNormalized;

        hurtbox.Enable();
        lightningSprite.enabled = true;
        SetColor(baseColor);

        while (timer < strikeDuration)
        {
            timer += Time.deltaTime;
            timerNormalized = Mathf.Clamp(timer / strikeDuration - 0.2f, 0.0f, 1.0f);
            SetColor(Color.Lerp(baseColor, transparentColor, timerNormalized));
            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }

    public void SetPositionToPlayer()
    {
        transform.position = new Vector2(GameManager.Instance.GetPlayerPosition().x, 0.0f);
    }

    public void FollowPlayer()
    {
        float currentPosX = transform.position.x;
        float targetPosX = GameManager.Instance.GetPlayerPosition().x;
        float newPosX = Mathf.Lerp(currentPosX, targetPosX, followAccel * Time.deltaTime);

        if (Mathf.Abs(newPosX - currentPosX) / Time.deltaTime >= followSpeedMax)
        {
            newPosX = currentPosX + Mathf.Sign(newPosX - currentPosX) * followSpeedMax * Time.deltaTime;
        }

        transform.position = new Vector2(newPosX, transform.position.y);
    }

    public void SetBaseColor(Color newBaseColor) => baseColor = newBaseColor;

    private void SetColor(Color newColor)
    {
        outlineSprite.color = newColor;
        lightningSprite.color = newColor;
        warningLabel.color = newColor;
    }

    public float GetTimeToStrike() => followDuration + warningDuration;
}
