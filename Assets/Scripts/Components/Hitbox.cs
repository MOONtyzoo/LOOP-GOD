using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private ImmunityLevels immunityLevel = ImmunityLevels.Ground;
    [SerializeField] private float immunityDuration = 0f;

    private SpriteRenderer spriteRenderer;
    private Collider2D boxCollider;

    private Coroutine flashCoroutine;

    private bool hasHitImmunity = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        spriteRenderer.color = defaultColor;
        spriteRenderer.enabled = debug;
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

    public void Hit(Hurtbox hurtbox)
    {
        if (debug)
        {
            if (flashCoroutine != null) StopCoroutine(flashCoroutine);
            StartCoroutine(FlashCoroutine());
        }
        if (immunityDuration > 0f) StartCoroutine(ImmunityCoroutine());
        OnHit?.Invoke();
    }

    public bool IsEnabled() => boxCollider.enabled;
    public void Enable()
    {
        boxCollider.enabled = true;
        spriteRenderer.color = defaultColor;
    }

    public void Disable()
    {
        boxCollider.enabled = false;
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

    private IEnumerator ImmunityCoroutine()
    {
        float timer = 0.0f;
        hasHitImmunity = true;
        while (timer < immunityDuration)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        hasHitImmunity = false;
    }

    public Teams GetTeam() => team;
    public ImmunityLevels GetImmunityLevel() => immunityLevel;
    public void SetImmunityLevel(ImmunityLevels newImmunityLevel) => immunityLevel = newImmunityLevel;
    public bool HasHitImmunity() => hasHitImmunity;
}

public enum Teams
{
    Player,
    Enemy,
}

public enum ImmunityLevels
{
    Ground,
    Air,
    FullImmunity
}
