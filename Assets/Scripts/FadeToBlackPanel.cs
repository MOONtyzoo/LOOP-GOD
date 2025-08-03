using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlackPanel : MonoBehaviour
{
    [SerializeField] private Color blackColor;
    [SerializeField] private Color transparentColor;
    [SerializeField] private float waitDuration;
    [SerializeField] private float fadeFromBlackDuration;
    [SerializeField] private float fadeToBlackDuration;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.color = blackColor;
    }

    private void Start()
    {
        FadeFromBlack();
    }

    public void FadeFromBlack()
    {
        StartCoroutine(FlashCoroutine(blackColor, transparentColor, fadeFromBlackDuration, waitDuration));
    }

    public void FadeToBlack()
    {
        StartCoroutine(FlashCoroutine(transparentColor, blackColor, fadeToBlackDuration));
    }

    private IEnumerator FlashCoroutine(Color fromColor, Color toColor, float fadeDuration, float waitDuration=0.0f)
    {
        float timer = 0.0f;
        float timerNormalized;
        yield return new WaitForSeconds(waitDuration);
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            timerNormalized = timer / fadeDuration;
            float cubicLerpVal = 1.0f + Mathf.Pow(timerNormalized - 1, 3f);

            Color newColor = Color.Lerp(fromColor, toColor, cubicLerpVal);
            image.color = newColor;

            yield return new WaitForEndOfFrame();
        }
    }
}
