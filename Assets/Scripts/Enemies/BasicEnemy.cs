using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private float speedGain;

    private Hitbox hitbox;

    private void Awake()
    {
        hitbox = GetComponentInChildren<Hitbox>();
        hitbox.OnHit += OnHit;
    }

    private void OnHit()
    {
        GameManager.Instance.EnemyKilled(speedGain);
        Destroy(gameObject);
    }
}
