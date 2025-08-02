using System;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    public event Action OnAmmoChanged;

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
    public void SetAmmo(int newAmmo)
    {
        int oldAmmo = ammo;
        ammo = Mathf.Clamp(newAmmo, 0, ammoMax);
        if (oldAmmo != ammo) OnAmmoChanged?.Invoke();
    }
    public int GetMaxAmmo() => ammoMax;
}
