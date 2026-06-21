using UnityEngine;

public class CoinPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private int coinValue = 1;
    [SerializeField] private GameObject interactionPrompt;

    public void Interact(PlayerInteractor player)
    {
        GameManager.Instance.AddCoins(coinValue);
        Destroy(gameObject);
    }

    public void ShowPrompt()
    {
        interactionPrompt.SetActive(true);
    }

    public void HidePrompt()
    {
        interactionPrompt.SetActive(false);
    }
}
