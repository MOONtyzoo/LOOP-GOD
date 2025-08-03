using TMPro;
using UnityEngine;

public class DebugPanel : MonoBehaviour
{
    [Header("Text Labels")]
    [SerializeField] private TextMeshProUGUI difficultyLevelLabel;

    private void Update()
    {
        difficultyLevelLabel.text = GameManager.Instance.GetDifficultyLevel().name;
    }
}
