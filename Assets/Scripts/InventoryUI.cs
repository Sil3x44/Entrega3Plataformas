using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("Weapon Icons")]
    [SerializeField] private Image swordIcon;
    [SerializeField] private Image spearIcon;
    [SerializeField] private Image bowIcon;
    [SerializeField] private Image equippedWeaponIcon;

    [Header("Potion Texts")]
    [SerializeField] private TextMeshProUGUI healthPotionCountText;
    [SerializeField] private TextMeshProUGUI damagePotionCountText;

    [Header("Coins")]
    [SerializeField] private TextMeshProUGUI coinsText;

    public void UpdateWeaponIcons(bool hasSword, bool hasSpear, bool hasBow)
    {
        swordIcon.color = hasSword ? Color.white : Color.black;
        spearIcon.color = hasSpear ? Color.white : Color.black;
        bowIcon.color = hasBow ? Color.white : Color.black;
    }

    public void UpdateEquippedWeapon(WeaponType currentWeapon, Sprite swordSprite, Sprite spearSprite, Sprite bowSprite)
    {
        equippedWeaponIcon.gameObject.SetActive(true);

        if (currentWeapon == WeaponType.Sword)
        {
            equippedWeaponIcon.sprite = swordSprite;
        }
        else if (currentWeapon == WeaponType.Spear)
        {
            equippedWeaponIcon.sprite = spearSprite;
        }
        else if (currentWeapon == WeaponType.Bow)
        {
            equippedWeaponIcon.sprite = bowSprite;
        }

        equippedWeaponIcon.color = Color.white;
    }

    public void UpdatePotionCounts(int healthPotions, int damagePotions)
    {
        healthPotionCountText.text = "x" + healthPotions;
        damagePotionCountText.text = "x" + damagePotions;
    }

    public void UpdateCoins(int coins)
    {
        coinsText.text = "x" + coins;
    }
    
    public void HideEquippedWeapon()
    {
        equippedWeaponIcon.gameObject.SetActive(false);
    }
}
