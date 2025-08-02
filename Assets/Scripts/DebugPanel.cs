using TMPro;
using UnityEngine;

public class DebugPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private PlayerGun playerGun;
    [SerializeField] private LoopGod loopGod;

    [Header("Text Labels")]
    [SerializeField] private TextMeshProUGUI speedLabel;
    [SerializeField] private TextMeshProUGUI distanceLabel;
    [SerializeField] private TextMeshProUGUI ammoLabel;

    private void Update()
    {
        speedLabel.text = "Speed: " + GameManager.Instance.GetSpeed().ToString("F2");
        distanceLabel.text = "Loop God Distance: " + loopGod.GetDistance().ToString("F2");
        ammoLabel.text = "Ammo: " + playerGun.GetAmmo() + " / " + playerGun.GetMaxAmmo();
    }
}
