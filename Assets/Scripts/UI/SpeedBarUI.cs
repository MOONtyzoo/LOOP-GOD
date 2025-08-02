using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBarUI : MonoBehaviour
{
    [SerializeField] private Image fillBar;
    [SerializeField] private TextMeshProUGUI speedLabel;

    private void Awake()
    {
        GameManager.Instance.OnSpeedChanged += GameManager_OnSpeedChanged;
    }

    private void GameManager_OnSpeedChanged(float newSpeed)
    {
        fillBar.fillAmount = newSpeed / GameManager.Instance.GetTerminalSpeed();
        speedLabel.SetText(newSpeed.ToString("F0"));
    }
}
