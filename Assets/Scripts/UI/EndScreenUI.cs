using System.Collections;
using TMPro;
using UnityEngine;

public class EndScreenUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lapsCompletedLabel;
    [SerializeField] private TextMeshProUGUI timeSurvivedLabel;
    [SerializeField] private float fadeInDelay;
    [SerializeField] private float fadeInDuration;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.0f;
    }

    public void OpenEndScreen()
    {
        lapsCompletedLabel.text = "Laps Completed: " + (GameManager.Instance.GetLap() - 1).ToString();
        timeSurvivedLabel.text = "Time Survived: " + GameManager.Instance.GetTimeSurvived().ToString("F2");
        StartCoroutine(FadeInCoroutine());
    }

    public IEnumerator FadeInCoroutine()
    {
        float timer = 0.0f;
        float timerNormalized;

        yield return new WaitForSeconds(fadeInDelay);
        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;
            timerNormalized = timer / fadeInDuration;
            canvasGroup.alpha = timerNormalized;
            yield return new WaitForEndOfFrame();
        }
    }
}
