using TMPro;
using UnityEngine;

public class DebugPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Player player;

    [Header("Text Labels")]
    [SerializeField] private TextMeshProUGUI speedLabel;
    [SerializeField] private TextMeshProUGUI distanceLabel;
    [SerializeField] private TextMeshProUGUI jumpValLabel;

    private void Update()
    {
        speedLabel.text = "Speed: " + GameManager.Instance.GetSpeed().ToString("F2");
        distanceLabel.text = "Distance: " + GameManager.Instance.GetDistance().ToString("F2");
        jumpValLabel.text = "JumpVal: " + player.GetJumpVal();
    }
}
