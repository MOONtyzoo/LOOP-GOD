using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlackPanel : MonoBehaviour
{
    [SerializeField] private Color blackColor;
    [SerializeField] private Color transparentColor;
    [SerializeField] private float waitDuration;
    [SerializeField] private float fadeDuration;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.color = blackColor;
    }

    private void Start()
    {
        StartCoroutine(FlashCoroutine(blackColor, transparentColor));
    }

    private IEnumerator FlashCoroutine(Color fromColor, Color toColor)
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
