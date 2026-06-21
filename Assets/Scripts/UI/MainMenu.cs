using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string villageSceneName = "Village";

    public void Play()
    {
        Time.timeScale = 1f;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetRun();
        }

        SceneManager.LoadScene(villageSceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
