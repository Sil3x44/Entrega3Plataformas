using UnityEngine;

public enum WeaponType
{
    Sword,
    Spear,
    Bow
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private InventoryUI inventoryUI;

    private bool hasSword;
    private bool hasSpear;
    private bool hasBow;

    private int healthPotions;
    private int damagePotions;
    private int coins;

    private WeaponType currentWeapon = WeaponType.Sword;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateUI();
    }

    public void CollectWeapon(WeaponType weaponType)
    {
        if (weaponType == WeaponType.Sword) hasSword = true;
        if (weaponType == WeaponType.Spear) hasSpear = true;
        if (weaponType == WeaponType.Bow) hasBow = true;

        if (HasWeapon(weaponType))
        {
            currentWeapon = weaponType;
        }

        UpdateUI();
    }

    public void ChangeWeapon(WeaponType weaponType)
    {
        if (!HasWeapon(weaponType)) return;

        currentWeapon = weaponType;
        UpdateUI();
    }

    public bool HasWeapon(WeaponType weaponType)
    {
        if (weaponType == WeaponType.Sword) return hasSword;
        if (weaponType == WeaponType.Spear) return hasSpear;
        if (weaponType == WeaponType.Bow) return hasBow;

        return false;
    }

    public bool HasAllWeapons()
    {
        return hasSword && hasSpear && hasBow;
    }

    public WeaponType GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public void AddHealthPotion()
    {
        healthPotions++;
        UpdateUI();
    }

    public void AddDamagePotion()
    {
        damagePotions++;
        UpdateUI();
    }
    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (inventoryUI == null) return;

        inventoryUI.UpdateWeaponIcons(hasSword, hasSpear, hasBow);
        inventoryUI.UpdatePotionIcons(healthPotions, damagePotions);
        inventoryUI.UpdateSelectedWeapon(currentWeapon);
        inventoryUI.UpdateCoins(coins);
    }
}