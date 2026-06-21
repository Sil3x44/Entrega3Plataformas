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

    [Header("Weapon Sprites")]
    [SerializeField] private Sprite swordSprite;
    [SerializeField] private Sprite spearSprite;
    [SerializeField] private Sprite bowSprite;
    
    [Header("Potion Settings")]
    [SerializeField] private int potionCost = 5;
    [SerializeField] private int healthPotionHealAmount = 2;
    [SerializeField] private float damagePotionDuration = 8f;

    private InventoryUI inventoryUI;
    
    private bool damageBoostActive;
    private bool hasSword;
    private bool hasSpear;
    private bool hasBow;

    private int coins;
    private int healthPotions;
    private int damagePotions;

    private WeaponType currentWeapon = WeaponType.None;

    private int playerCurrentHealth;
    private int playerMaxHealth;

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

    public void RegisterPlayerHealth(int maxHealth)
    {
        playerMaxHealth = maxHealth;

        if (playerCurrentHealth <= 0)
        {
            playerCurrentHealth = playerMaxHealth;
        }
    }

    public int GetPlayerCurrentHealth()
    {
        return playerCurrentHealth;
    }

    public void SetPlayerCurrentHealth(int value)
    {
        playerCurrentHealth = value;
    }

    public void ResetRun()
    {
        hasSword = false;
        hasSpear = false;
        hasBow = false;

        coins = 0;
        healthPotions = 0;
        damagePotions = 0;

        currentWeapon = WeaponType.None;

        playerCurrentHealth = playerMaxHealth;

        UpdateUI();
    }

    public void CollectWeapon(WeaponType weaponType)
    {
        if (weaponType == WeaponType.Sword) hasSword = true;
        if (weaponType == WeaponType.Spear) hasSpear = true;
        if (weaponType == WeaponType.Bow) hasBow = true;

        currentWeapon = weaponType;

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

    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateUI();
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
    
    public bool TryUseHealthPotion(PlayerHealth playerHealth)
    {
        if (healthPotions > 0)
        {
            healthPotions--;
            playerHealth.Heal(healthPotionHealAmount);
            UpdateUI();
            return true;
        }

        if (coins >= potionCost)
        {
            coins -= potionCost;
            healthPotions++;
            UpdateUI();
            return false;
        }

        return false;
    }

    public bool TryUseDamagePotion()
    {
        if (damagePotions > 0)
        {
            damagePotions--;
            damageBoostActive = true;
            UpdateUI();
            return true;
        }

        if (coins >= potionCost)
        {
            coins -= potionCost;
            damagePotions++;
            UpdateUI();
            return false;
        }

        return false;
    }

    public bool GetDamageBoostActive()
    {
        return damageBoostActive;
    }

    public void DisableDamageBoost()
    {
        damageBoostActive = false;
    }
    
    public void SetInventoryUI(InventoryUI newInventoryUI)
    {
        inventoryUI = newInventoryUI;
        UpdateUI();
    }

    public void ShowDamagePotionTimer(float timeLeft)
    {
        inventoryUI.ShowDamagePotionTimer(timeLeft);
    }

    public void HideDamagePotionTimer()
    {
        inventoryUI.HideDamagePotionTimer();
    }

    private void UpdateUI()
    {
        if (inventoryUI == null) return;

        inventoryUI.UpdateWeaponIcons(hasSword, hasSpear, hasBow);
        inventoryUI.UpdatePotionCounts(healthPotions, damagePotions);
        inventoryUI.UpdateCoins(coins);

        if (HasWeapon(currentWeapon))
        {
            inventoryUI.UpdateEquippedWeapon(currentWeapon, swordSprite, spearSprite, bowSprite);
        }
        else
        {
            inventoryUI.HideEquippedWeapon();
        }
    }
}