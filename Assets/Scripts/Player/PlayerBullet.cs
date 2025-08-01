using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float speed;

    private Hurtbox hurtbox;

    private void Awake()
    {
        hurtbox = GetComponentInChildren<Hurtbox>();
        hurtbox.OnHit += Hurtbox_OnHit;
    }

    private void Hurtbox_OnHit()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        float newX = transform.position.x + speed * Time.deltaTime;
        transform.position = new Vector2(newX, transform.position.y);
    }
}
