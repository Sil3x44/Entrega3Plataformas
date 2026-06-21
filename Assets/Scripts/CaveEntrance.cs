using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveEntrance : MonoBehaviour, IInteractable
{
    [SerializeField] private string caveSceneName;
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private GameObject lockedMessage;

    public void Interact(PlayerInteractor player)
    {
        if (GameManager.Instance.HasAllWeapons())
        {
            SceneManager.LoadScene(caveSceneName);
        }
        else
        {
            lockedMessage.SetActive(true);
        }
    }

    public void ShowPrompt()
    {
        interactionPrompt.SetActive(true);
    }

    public void HidePrompt()
    {
        interactionPrompt.SetActive(false);
        lockedMessage.SetActive(false);
    }
}
