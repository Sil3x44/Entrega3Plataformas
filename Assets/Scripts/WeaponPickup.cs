using UnityEngine;

public class WeaponPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private GameObject interactionPrompt;

    public void Interact(PlayerInteractor player)
    {
        GameManager.Instance.CollectWeapon(weaponType);
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
