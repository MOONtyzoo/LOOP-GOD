using System;
using System.Collections;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public event Action OnHit;

    [Header("Debug")]
    [SerializeField] private bool debug = false;
    [SerializeField] private float flashDuration = 0.5f;
    [SerializeField] private Color flashColor;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color disabledColor;

    [Header("Behavior")]
    [SerializeField] private Teams team;


    private SpriteRenderer spriteRenderer;
    private new Collider2D collider;

    private Coroutine flashCoroutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = defaultColor;
        collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (debug)
        {
            spriteRenderer.enabled = true;
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }

    public void Hit()
    {
        if (debug)
        {
            if (flashCoroutine != null) StopCoroutine(flashCoroutine);
            StartCoroutine(FlashCoroutine());
        }
        OnHit?.Invoke();
    }

    public bool IsEnabled() => collider.enabled;
    public void Enable()
    {
        collider.enabled = true;
        spriteRenderer.color = defaultColor;
    }

    public void Disable()
    {
        collider.enabled = false;
        spriteRenderer.color = disabledColor;
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

            Color newColor = Color.Lerp(flashColor, defaultColor, cubicLerpVal);
            spriteRenderer.color = newColor;

            yield return new WaitForEndOfFrame();
        }
    }

    public Teams GetTeam() => team;
}

public enum Teams
{
    Player,
    Enemy,
}
