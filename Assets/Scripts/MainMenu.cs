using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text waveCountText;

    public void MainMenuless()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GameScene()
    {
        SceneManager.LoadScene("Main");
    }

    public void ArenaScene()
    {
        SceneManager.LoadScene("Arena");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void HowToPlay()
    {
        SceneManager.LoadScene("HowToPlay");
    }

    public void SaveScene()
    {
        PlayerPrefs.SetString("SavedScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }

    public void LoadScene()
    {
        string savedScene = PlayerPrefs.GetString("SavedScene");
        if (!string.IsNullOrEmpty(savedScene))
        {
            SceneManager.LoadScene(savedScene);
        }
    }
}