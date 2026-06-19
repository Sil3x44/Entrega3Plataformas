using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius;
    [SerializeField] private int playerBaseDamage;
    [SerializeField] private int playerCritDamage;

    private Collider2D[] targetsHit;
    private float comboCooldown;
    private bool hasAttackedOnce;
    private bool hasAttackedTwice;
    private bool isParryable;
    
    private void Update()
    {
        UpdateAttack();
    }

    private void UpdateAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!hasAttackedOnce && !hasAttackedTwice)
            {
                hasAttackedOnce = true;
                //play attack1 animation with event
            }
            else if (hasAttackedOnce && !hasAttackedTwice)
            {
                hasAttackedTwice = true;
                //play attack2 animation with event
            }
            else
            {
                hasAttackedOnce = false;
                hasAttackedTwice = false;
                //play attack3 animation with event
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            PerformParry();
        }
    }

    private void PerformAttack()
    {
        targetsHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius);

        foreach (Collider2D target in targetsHit)
        {
            if (TryGetComponent(out IDamagable damage))
            {
                damage.TakeDamage(playerBaseDamage);
            }
        }
    }

    private void PerformParry()
    {
        targetsHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius);
        
        foreach (Collider2D target in targetsHit)
        {
            if (TryGetComponent(out IParryable parryable) && TryGetComponent(out IDamagable damage))
            {
                parryable.CheckForParry(isParryable);
                damage.TakeDamage(playerCritDamage);
            }
        }
    }
    
    
}
