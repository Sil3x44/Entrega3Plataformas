using UnityEngine;

public enum WeaponType
{
    None,
    Sword,
    Spear,
    Bow
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private InventoryUI inventoryUI;

    [Header("Weapon Sprites")]
    [SerializeField] private Sprite swordSprite;
    [SerializeField] private Sprite spearSprite;
    [SerializeField] private Sprite bowSprite;

    private bool hasSword;
    private bool hasSpear;
    private bool hasBow;

    private int healthPotions;
    private int damagePotions;
    private int coins;

    private WeaponType currentWeapon = WeaponType.None;

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

        FindFirstObjectByType<PlayerWeaponAnimations>().UpdateAnimationSet();

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
        inventoryUI.UpdateWeaponIcons(hasSword, hasSpear, hasBow);
        inventoryUI.UpdatePotionCounts(healthPotions, damagePotions);
        inventoryUI.UpdateCoins(coins);

        if (HasWeapon(currentWeapon))
        {
            inventoryUI.UpdateEquippedWeapon(
                currentWeapon,
                swordSprite,
                spearSprite,
                bowSprite);
        }
        else
        {
            inventoryUI.HideEquippedWeapon();
        }
    }
}