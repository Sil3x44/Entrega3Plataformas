using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private string villageSceneName = "Village";
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private void Start()
    {
        gameOverPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        AudioManager.Instance.PlayGameOverMusic();
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartVillage()
    {
        Time.timeScale = 1f;

        GameManager.Instance.ResetRun();

        SceneManager.LoadScene(villageSceneName);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;

        GameManager.Instance.ResetRun();

        SceneManager.LoadScene(mainMenuSceneName);
    }
}
