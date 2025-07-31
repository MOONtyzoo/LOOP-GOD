using TMPro;
using UnityEngine;

public class DebugPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedLabel;
    [SerializeField] private TextMeshProUGUI distanceLabel;

    private void Update()
    {
        speedLabel.text = "Speed: " + GameManager.Instance.GetSpeed().ToString("F2");
        distanceLabel.text = "Distance: " + GameManager.Instance.GetDistance().ToString("F2");
    }
}
