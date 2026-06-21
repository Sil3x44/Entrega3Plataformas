using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("Weapon Icons")]
    [SerializeField] private Image swordIcon;
    [SerializeField] private Image spearIcon;
    [SerializeField] private Image bowIcon;

    [Header("Weapon Selection Frames")]
    [SerializeField] private GameObject swordSelectedFrame;
    [SerializeField] private GameObject spearSelectedFrame;
    [SerializeField] private GameObject bowSelectedFrame;

    [Header("Potion Icons")]
    [SerializeField] private Image healthPotionIcon;
    [SerializeField] private Image damagePotionIcon;
    [SerializeField] private TextMeshProUGUI healthPotionCountText;
    [SerializeField] private TextMeshProUGUI damagePotionCountText;
    
    [Header("Coin Text")]
    [SerializeField] private TextMeshProUGUI coinsText;

    [Header("Alpha")]
    [SerializeField] private float lockedAlpha = 0.3f;
    [SerializeField] private float unlockedAlpha = 1f;

    public void UpdateWeaponIcons(bool hasSword, bool hasSpear, bool hasBow)
    {
        SetIconAlpha(swordIcon, hasSword ? unlockedAlpha : lockedAlpha);
        SetIconAlpha(spearIcon, hasSpear ? unlockedAlpha : lockedAlpha);
        SetIconAlpha(bowIcon, hasBow ? unlockedAlpha : lockedAlpha);
    }

    public void UpdatePotionIcons(int healthPotions, int damagePotions)
    {
        SetIconAlpha(healthPotionIcon, healthPotions > 0 ? unlockedAlpha : lockedAlpha);
        SetIconAlpha(damagePotionIcon, damagePotions > 0 ? unlockedAlpha : lockedAlpha);

        healthPotionCountText.text = "x" + healthPotions;
        damagePotionCountText.text = "x" + damagePotions;
    }

    public void UpdateSelectedWeapon(WeaponType weaponType)
    {
        swordSelectedFrame.SetActive(weaponType == WeaponType.Sword);
        spearSelectedFrame.SetActive(weaponType == WeaponType.Spear);
        bowSelectedFrame.SetActive(weaponType == WeaponType.Bow);
    }
    
    public void UpdateCoins(int coins)
    {
        coinsText.text = "x" + coins;
    }

    private void SetIconAlpha(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}
