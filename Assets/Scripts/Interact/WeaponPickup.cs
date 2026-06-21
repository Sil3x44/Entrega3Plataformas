using UnityEngine;

public class WeaponPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private GameObject collectEffectPrefab;

    public void Interact(PlayerInteractor player)
    {
        if (collectEffectPrefab != null)
        {
            Instantiate(collectEffectPrefab, transform.position, Quaternion.identity);
        }

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
