using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 5;

    [Header("References")]
    [SerializeField] private PlayerHealthUI playerHealthUI;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Damage Feedback")]
    [SerializeField] private float flashDelay = 1f;
    [SerializeField] private float flashTime = 0.12f;

    [Header("Death")]
    [SerializeField] private float gameOverDelay = 1f;
    
    private PlayerMovement playerMovement;
    private PlayerCombat playerCombat;
    private Rigidbody2D rb;

    private int currentHealth;
    private bool isDead;
    private Color originalColor;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerCombat = GetComponent<PlayerCombat>();
        rb = GetComponent<Rigidbody2D>();
        originalColor = spriteRenderer.color;
    }

    private void Start()
    {
        GameManager.Instance.RegisterPlayerHealth(maxHealth);

        currentHealth = GameManager.Instance.GetPlayerCurrentHealth();

        if (currentHealth <= 0)
        {
            currentHealth = maxHealth;
        }

        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        GameManager.Instance.SetPlayerCurrentHealth(currentHealth);

        UpdateHealthUI();
        StartCoroutine(DamageFlashCoroutine());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        GameManager.Instance.SetPlayerCurrentHealth(currentHealth);

        UpdateHealthUI();
    }

    private IEnumerator DamageFlashCoroutine()
    {
        yield return new WaitForSeconds(flashDelay);

        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(flashTime);

        spriteRenderer.color = originalColor;
    }

    private void UpdateHealthUI()
    {
        playerHealthUI.UpdateHearts(currentHealth);
    }

    private void Die()
    {
        isDead = true;

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        playerMovement.SetControlsLocked(true);
        playerCombat.enabled = false;

        animator.SetTrigger("Die");

        StartCoroutine(ShowGameOverAfterDelay());
    }

    private IEnumerator ShowGameOverAfterDelay()
    {
        yield return new WaitForSeconds(gameOverDelay);

        GameOverMenu gameOverMenu = FindFirstObjectByType<GameOverMenu>();

        if (gameOverMenu != null)
        {
            gameOverMenu.ShowGameOver();
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public bool GetIsDead()
    {
        return isDead;
    }
}
