using UnityEngine;

public enum PotionType
{
    Health,
    Damage
}

public class PotionPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private PotionType potionType;
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private GameObject collectEffectPrefab;

    public void Interact(PlayerInteractor player)
    {
        Instantiate(collectEffectPrefab, transform.position, Quaternion.identity);

        if (potionType == PotionType.Health)
        {
            GameManager.Instance.AddHealthPotion();
        }
        else if (potionType == PotionType.Damage)
        {
            GameManager.Instance.AddDamagePotion();
        }

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
