using TMPro;
using UnityEngine;

public class SignInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private GameObject textBubble;
    [SerializeField] private TextMeshPro signText;
    [SerializeField] private string message;

    private bool isOpen;

    public void Interact(PlayerInteractor player)
    {
        isOpen = !isOpen;

        textBubble.SetActive(isOpen);
        signText.text = message;
    }

    public void ShowPrompt()
    {
        interactionPrompt.SetActive(true);
    }

    public void HidePrompt()
    {
        interactionPrompt.SetActive(false);
        textBubble.SetActive(false);
        isOpen = false;
    }
}
