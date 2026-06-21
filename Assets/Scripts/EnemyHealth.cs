using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 10;

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Damage Feedback")]
    [SerializeField] private float flashDelay = 1f;
    [SerializeField] private float flashTime = 0.12f;

    [Header("Death")]
    [SerializeField] private float destroyDelay = 1f;

    private int currentHealth;
    private bool isDead;
    private Color originalColor;

    private void Awake()
    {
        currentHealth = maxHealth;
        originalColor = spriteRenderer.color;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        StartCoroutine(DamageFlashCoroutine());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator DamageFlashCoroutine()
    {
        yield return new WaitForSeconds(flashDelay);

        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(flashTime);

        spriteRenderer.color = originalColor;
    }

    private void Die()
    {
        isDead = true;

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        animator.SetTrigger("Die");

        Destroy(gameObject, destroyDelay);
    }

    public bool GetIsDead()
    {
        return isDead;
    }
}
