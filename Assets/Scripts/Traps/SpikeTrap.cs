using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float damageCooldown = 0.5f;

    private bool canDamage = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryDamage(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryDamage(other);
    }

    private void TryDamage(Collider2D other)
    {
        if (!canDamage) return;

        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
            StartCoroutine(DamageCooldownCoroutine());
        }
    }

    private System.Collections.IEnumerator DamageCooldownCoroutine()
    {
        canDamage = false;

        yield return new WaitForSeconds(damageCooldown);

        canDamage = true;
    }
}