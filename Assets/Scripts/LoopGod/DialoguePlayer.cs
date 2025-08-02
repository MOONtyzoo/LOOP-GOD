using System;
using System.Collections;
using Febucci.UI.Core;
using UnityEngine;

public class LoopGodDialogue : MonoBehaviour
{
    public event Action OnDialogueEnded;

    [Header("References")]
    [SerializeField] private TypewriterCore typewriter;
    private Animator animator;

    [Header("Data")]
    [SerializeField] private float timeBetweenLines;
    [SerializeField] private float textBoxAppearRate;

    [Header("Dialogue Fields")]
    [SerializeField] private Dialogue debugDialogue;

    private Dialogue currentDialogue;
    private int lineCount;
    private int dialogueIndex;
    private bool isDialogueActive;
    private bool typewriterShowCompleted;

    private float targetShowAmount;
    private float showAmount = 0.0f;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        typewriter.onTextShowed.AddListener(() => typewriterShowCompleted = true);
    }

    public void Start()
    {
        Hide();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(PlayDialogueCoroutine(debugDialogue));
        }

        showAmount = Mathf.Lerp(showAmount, targetShowAmount, Time.deltaTime * textBoxAppearRate);
        animator.SetFloat("ShowAmount", showAmount);
    }

    private IEnumerator PlayDialogueCoroutine(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        ShowDialogue();

        while (dialogueIndex < lineCount)
        {
            AdvanceDialogue();
            yield return new WaitUntil(() => typewriterShowCompleted);
            yield return new WaitForSeconds(timeBetweenLines);
        }

        EndDialogue();
    }

    public void ShowDialogue()
    {
        Show();
        lineCount = currentDialogue.lines.Count;
        dialogueIndex = 0;
        isDialogueActive = true;
    }

    public void AdvanceDialogue()
    {
        DialogueLine nextDialogueLine = currentDialogue.lines[dialogueIndex];
        typewriter.ShowText(nextDialogueLine.text);
        typewriter.StartShowingText();
        typewriterShowCompleted = false;
        dialogueIndex++;
    }

    private void EndDialogue()
    {
        currentDialogue = null;
        isDialogueActive = false;
        OnDialogueEnded?.Invoke();
        Hide();
    }

    private void Show()
    {
        typewriter.gameObject.SetActive(true);
        targetShowAmount = 1f;
    }

    private void Hide()
    {
        typewriter.gameObject.SetActive(false);
        targetShowAmount = 0f;
    }
}
