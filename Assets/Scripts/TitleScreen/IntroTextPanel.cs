using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IntroTextPanel : MonoBehaviour
{
    [SerializeField] private Dialogue introDialogue;
    [SerializeField] private float flashDuration;
    [SerializeField] private Color flashColor;
    [SerializeField] private Color transparentColor;

    private DialoguePlayer dialoguePlayer;
    private Image image;

    private void Awake()
    {
        dialoguePlayer = GetComponentInChildren<DialoguePlayer>();
        image = GetComponent<Image>();
    }

    private void Start()
    {
        StartCoroutine(IntroCoroutine());
    }

    private IEnumerator IntroCoroutine()
    {
        yield return new WaitForSeconds(1f);
        dialoguePlayer.Play(introDialogue);
        dialoguePlayer.OnDialogueEnded += () => StartCoroutine(FlashCoroutine());
    }
    
    private IEnumerator FlashCoroutine()
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
