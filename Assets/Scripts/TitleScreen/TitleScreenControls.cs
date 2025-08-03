using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenControls : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Toggle SkipTutorialToggle;

    private void Awake()
    {
        playButton.onClick.AddListener(StartGame);
        SkipTutorialToggle.onValueChanged.AddListener(OnSkipTutorialToggled);

        SkipTutorialToggle.isOn = PlayerPrefs.GetInt("SkipTutorial?") == 1;
    }

    private void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    private void OnSkipTutorialToggled(bool val)
    {
        PlayerPrefs.SetInt("SkipTutorial?", val ? 1 : 0);
        PlayerPrefs.Save();
    }
}
