using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBarUI : MonoBehaviour
{
    [SerializeField] private LoopGod loopGod;
    [SerializeField] private Image fillBar;
    [SerializeField] private TextMeshProUGUI speedLabel;

    [Space]
    [SerializeField] private Image loopGodSpeedMarker;
    [SerializeField] private float zeroSpeedPosY;
    [SerializeField] private float maxSpeedPosY;

    private void Awake()
    {
        GameManager.Instance.OnSpeedChanged += GameManager_OnSpeedChanged;
        loopGod.OnFollowSpeedChanged += LoopGod_OnFollowSpeedChanged;
    }

    private void GameManager_OnSpeedChanged(float newSpeed)
    {
        fillBar.fillAmount = newSpeed / GameManager.Instance.GetTerminalSpeed();
        speedLabel.SetText(newSpeed.ToString("F0"));
    }

    private void LoopGod_OnFollowSpeedChanged(float newFollowSpeed)
    {
        float newPosY = Mathf.Lerp(zeroSpeedPosY, maxSpeedPosY, newFollowSpeed / GameManager.Instance.GetTerminalSpeed());
        loopGodSpeedMarker.rectTransform.localPosition = new Vector2(loopGodSpeedMarker.rectTransform.localPosition.x, newPosY);
        
    }
}
