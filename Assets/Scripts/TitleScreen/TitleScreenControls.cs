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
    }

    private void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
