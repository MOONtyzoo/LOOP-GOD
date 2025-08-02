using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LapProgressBarUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lapLabel;
    [SerializeField] private Image progressBar;

    private void Awake()
    {
        GameManager.Instance.OnLapChanged += GameManager_OnLapChanged;
        GameManager_OnLapChanged(1);
    }

    private void Update()
    {
        progressBar.fillAmount = GameManager.Instance.GetProgressToNextLap();
    }

    private void GameManager_OnLapChanged(int newLap)
    {
        lapLabel.text = "<wave>LAP: " + newLap.ToString() + "</wave>";
    }
}
