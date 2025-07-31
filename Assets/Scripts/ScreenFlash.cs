using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HurtScreenFlash : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Color hurtFlashColor;
    [SerializeField] private Color speedFlashColor;
    [SerializeField] private Color transparentColor;
    [SerializeField] private float flashDuration;

    private Coroutine flashCoroutine;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.color = transparentColor;

        player.OnHit += Player_OnHit;
        GameManager.Instance.OnEnemyKilled += GameManager_OnEnemyKilled;
    }

    private void Player_OnHit()
    {
        if (flashCoroutine != null) StopCoroutine(flashCoroutine);
        StartCoroutine(FlashCoroutine(hurtFlashColor));
    }

    private void GameManager_OnEnemyKilled()
    {
        if (flashCoroutine != null) StopCoroutine(flashCoroutine);
        StartCoroutine(FlashCoroutine(speedFlashColor));
    }

    private IEnumerator FlashCoroutine(Color flashColor)
    {
        float timer = 0.0f;
        float timerNormalized;
        while (timer < flashDuration)
        {
            timer += Time.deltaTime;
            timerNormalized = timer / flashDuration;
            float cubicLerpVal = 1.0f + Mathf.Pow(timerNormalized - 1, 3f);

            Color newColor = Color.Lerp(flashColor, transparentColor, cubicLerpVal);
            image.color = newColor;

            yield return new WaitForEndOfFrame();
        }
    }
}
