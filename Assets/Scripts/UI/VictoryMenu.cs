using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryMenu : MonoBehaviour
{
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string villageSceneName = "Village";

    private void Start()
    {
        victoryPanel.SetActive(false);
    }

    public void ShowVictory()
    {
        AudioManager.Instance.PlayVictoryMusic();
        victoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        GameManager.Instance.ResetRun();
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void RetryFromVillage()
    {
        Time.timeScale = 1f;
        GameManager.Instance.ResetRun();
        SceneManager.LoadScene(villageSceneName);
    }
}
