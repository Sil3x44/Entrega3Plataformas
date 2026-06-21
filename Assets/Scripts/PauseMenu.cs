using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    private bool isPaused;

    private void Start()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Pause()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    private void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void Retry()
    {
        Time.timeScale = 1;
        GameManager.Instance.ResetRun();
        SceneManager.LoadScene("Village");
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        GameManager.Instance.ResetRun();
        SceneManager.LoadScene("MainMenu");
    }
}
