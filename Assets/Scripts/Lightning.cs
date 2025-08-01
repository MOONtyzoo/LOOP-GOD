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

    [Header("Stats")]
    [SerializeField] private float followDuration;
    [SerializeField] private float warningDuration;
    [SerializeField] private float strikeDuration;
    [SerializeField] private float followAccel;
    [SerializeField] private float followSpeedMax;

    [Header("Visual")]
    [SerializeField] private float fadeInDuration;
    [SerializeField] private Color transparentColor;
    [SerializeField] private float gradientColorFrequency;
    [SerializeField] private Color gradientColor1;
    [SerializeField] private Color gradientColor2;
    [SerializeField] private float warningFlashTime;
    [SerializeField] private Color warningFlashColor;
    [SerializeField] private Color strikeColor;

    private float randomNum;

    private States currentState;
    private enum States
    {
        Follow,
        Warning,
        Strike
    }

    private void Start()
    {
        randomNum = Random.Range(0.0f, 99999.9f);
        SetColor(transparentColor);

        SetPositionToPlayer();
        StartCoroutine(FollowCoroutine());

        hurtbox.Disable();
    }

    private IEnumerator FollowCoroutine()
    {
        float timer = 0.0f;
        float fadeTimerNormalized;

        while (timer < followDuration)
        {
            timer += Time.deltaTime;
            fadeTimerNormalized = Mathf.Clamp(timer / fadeInDuration, 0.0f, 1.0f);
            SetColor(Color.Lerp(transparentColor, GetGradientColor(), fadeTimerNormalized));
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
                SetColor(GetGradientColor());
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
        SetColor(strikeColor);

        while (timer < strikeDuration)
        {
            timer += Time.deltaTime;
            timerNormalized = Mathf.Clamp(timer / strikeDuration - 0.2f, 0.0f, 1.0f);
            SetColor(Color.Lerp(GetGradientColor(), transparentColor, timerNormalized));
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

    private Color GetGradientColor()
    {
        float lerpVal = 0.5f*Mathf.Sin(Time.time*gradientColorFrequency + randomNum) + 0.5f;
        return Color.Lerp(gradientColor1, gradientColor2, lerpVal);
    }

    private void SetColor(Color newColor)
    {
        outlineSprite.color = newColor;
        lightningSprite.color = newColor;
        warningLabel.color = newColor;
    }
}
