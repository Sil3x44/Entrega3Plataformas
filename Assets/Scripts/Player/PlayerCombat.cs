using System;
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
    [SerializeField] private float arrowShootDelay = 0.8f;

    [Header("General Attack")]
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private float damageDelay = 0.15f;
    [SerializeField] private LayerMask enemyLayer;

    private PlayerMovement playerMovement;
    private bool canAttack = true;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

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

        WeaponType currentWeapon = GameManager.Instance.GetCurrentWeapon();

        if (currentWeapon == WeaponType.Bow)
        {
            yield return new WaitForSeconds(arrowShootDelay);

            ShootArrow();
        }
        else
        {
            yield return new WaitForSeconds(damageDelay);

            if (currentWeapon == WeaponType.Sword)
            {
                PerformMeleeAttack(swordRadius, swordDamage);
            }
            else if (currentWeapon == WeaponType.Spear)
            {
                PerformMeleeAttack(spearRadius, spearDamage);
            }
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
        GameObject arrow = Instantiate(arrowPrefab, bowShootPoint.position, Quaternion.identity);
        ArrowProjectile projectile = arrow.GetComponent<ArrowProjectile>();

        projectile.SetDamage(GetFinalDamage(bowDamage));
        projectile.SetDirection(playerMovement.GetFacingDirection());
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