using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool debug = false;
    [SerializeField] private float flashDuration = 0.5f;
    [SerializeField] private Color flashColor;
    [SerializeField] private Color defaultColor;

    [Header("Behavior")]
    [SerializeField] private Teams team;
    [SerializeField] private LayerMask hitboxLayerMask;
    [SerializeField] private ImmunityLevels immunityBreak;
    [SerializeField] private bool disableOnHit = false;

    private SpriteRenderer spriteRenderer;
    private new Collider2D collider;

    private Coroutine flashCoroutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        spriteRenderer.color = defaultColor;
        spriteRenderer.enabled = debug && IsEnabled();
    }

    private void Update()
    {
        spriteRenderer.enabled = debug && IsEnabled();

        if (IsEnabled())
        {
            SearchForHitboxes();
        }
    }

    public void SearchForHitboxes()
    {
        if (TryGetHitbox(out Hitbox hitbox))
        {
            hitbox.Hit();
            if (debug)
            {
                if (flashCoroutine != null) StopCoroutine(flashCoroutine);
                StartCoroutine(FlashCoroutine());
            }
            if (disableOnHit) Disable();
        }
    }

    public bool TryGetHitbox(out Hitbox hitbox)
    {
        List<Collider2D> results = new List<Collider2D>();
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.NoFilter();
        contactFilter.SetLayerMask(hitboxLayerMask);

        collider.Overlap(contactFilter, results);

        foreach (Collider2D result in results)
        {
            if (result.GetComponent<Hitbox>() != null)
            {
                hitbox = result.GetComponent<Hitbox>();
                bool isDifferentTeam = hitbox.GetTeam() != team;
                bool canBreakImmunity = immunityBreak >= hitbox.GetImmunityLevel();
                if (isDifferentTeam && canBreakImmunity) return true;
            }
        }

        hitbox = null;
        return false;
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
