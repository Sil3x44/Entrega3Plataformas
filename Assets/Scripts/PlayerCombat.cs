using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Animator animator;

    [Header("Attack")]
    [SerializeField] private float attackRadius = 0.7f;
    [SerializeField] private int playerBaseDamage = 10;
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

        PerformAttack();

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }

    private void PerformAttack()
    {
        Collider2D[] targetsHit = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRadius,
            enemyLayer);

        foreach (Collider2D target in targetsHit)
        {
            if (target.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(playerBaseDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}