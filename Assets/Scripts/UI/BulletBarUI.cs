using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletBarUI : MonoBehaviour
{
    [SerializeField] private List<Image> bulletSlots = new List<Image>(6);
    [SerializeField] private Sprite bulletSlotEmptySprite;
    [SerializeField] private Sprite bulletSlotFullSprite;

    [SerializeField] private PlayerGun playerGun;

    private void Awake()
    {
        playerGun.OnAmmoChanged += PlayerGun_OnAmmoChanged;
    }

    private void PlayerGun_OnAmmoChanged()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        for (int i = 0; i < bulletSlots.Count; i++)
        {
            Image bulletSlot = bulletSlots[i];
            Sprite oldSprite = bulletSlot.sprite;
            Sprite newSprite = (i + 1) <= playerGun.GetAmmo() ? bulletSlotFullSprite : bulletSlotEmptySprite;
            bulletSlot.sprite = newSprite;
        }
    }
}
