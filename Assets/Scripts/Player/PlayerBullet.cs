using System.Collections;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float destroyPosX;

    private Hurtbox hurtbox;

    private void Awake()
    {
        hurtbox = GetComponentInChildren<Hurtbox>();
        hurtbox.OnHit += Hurtbox_OnHit;
    }

    private void Hurtbox_OnHit()
    {
        StartCoroutine(ReactivateHurtboxCoroutine());
    }

    private IEnumerator ReactivateHurtboxCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        hurtbox.Enable();
    }

    private void Update()
    {
        float newX = transform.position.x + speed * Time.deltaTime;
        transform.position = new Vector2(newX, transform.position.y);

        if (transform.position.x > destroyPosX)
        {
            Destroy(gameObject);
        }
    }
}
