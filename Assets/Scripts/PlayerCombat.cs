using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Transform bowShootPoint;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject hitEffectPrefab;

    [Header("Sword")]
    [SerializeField] private float swordRadius = 0.7f;
    [SerializeField] private int swordDamage = 10;

    [Header("Spear")]
    [SerializeField] private float spearRadius = 1.2f;
    [SerializeField] private int spearDamage = 8;

    [Header("Bow")]
    [SerializeField] private int bowDamage = 5;

    [Header("General Attack")]
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private float damageDelay = 0.15f;
    [SerializeField] private LayerMask enemyLayer;

    private bool canAttack = true;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            StartCoroutine(AttackCoroutine());
        }
    }

    private IEnumerator AttackCoroutine()
    {
        canAttack = false;

        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(damageDelay);

        WeaponType currentWeapon = GameManager.Instance.GetCurrentWeapon();

        if (currentWeapon == WeaponType.Sword)
        {
            PerformMeleeAttack(swordRadius, swordDamage);
        }
        else if (currentWeapon == WeaponType.Spear)
        {
            PerformMeleeAttack(spearRadius, spearDamage);
        }
        else if (currentWeapon == WeaponType.Bow)
        {
            ShootArrow();
        }

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }

    private void PerformMeleeAttack(float radius, int damage)
    {
        int finalDamage = GetFinalDamage(damage);

        Collider2D[] targetsHit = Physics2D.OverlapCircleAll(attackPoint.position, radius, enemyLayer);

        foreach (Collider2D target in targetsHit)
        {
            if (target.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(finalDamage);
                Instantiate(hitEffectPrefab, target.transform.position, Quaternion.identity);
            }
        }
    }

    private void ShootArrow()
    {
        GameObject arrow = Instantiate(
            arrowPrefab,
            bowShootPoint.position,
            bowShootPoint.rotation);

        ArrowProjectile projectile = arrow.GetComponent<ArrowProjectile>();

        int finalDamage = GetFinalDamage(bowDamage);
        projectile.SetDamage(finalDamage);

        float directionX = transform.rotation.y == 0 ? 1f : -1f;
        projectile.SetDirection(directionX);
    }

    private int GetFinalDamage(int baseDamage)
    {
        if (GameManager.Instance.GetDamageBoostActive())
        {
            return baseDamage * 2;
        }

        return baseDamage;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, swordRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, spearRadius);
    }
}