using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] private int ammoMax;
    [SerializeField] private PlayerBullet bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;

    private int ammo;

    public void Shoot()
    {
        SetAmmo(ammo - 1);
        PlayerBullet bullet = Instantiate(bulletPrefab);
        bullet.transform.position = bulletSpawnPoint.position;
    }

    public void Reload()
    {
        SetAmmo(ammo + 1);
    }

    public bool HasAmmo() => ammo > 0;
    public int GetAmmo() => ammo;
    public int SetAmmo(int newAmmo) => ammo = Mathf.Clamp(newAmmo, 0, ammoMax);
    public int GetMaxAmmo() => ammoMax;
}
