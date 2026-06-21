using System.Collections;
using UnityEngine;

public class SkeletonEnemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform visual;
    [SerializeField] private GameObject alertIcon;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Animator animator;

    [Header("Patrol")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float patrolSpeed = 2f;

    [Header("Detection")]
    [SerializeField] private float detectionRange = 6f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Chase")]
    [SerializeField] private float chaseSpeed = 3.5f;
    [SerializeField] private float stopDistance = 1.2f;

    [Header("Attack")]
    [SerializeField] private float attackRadius = 0.7f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackCooldown = 2f;

    private Transform currentPatrolPoint;
    private Rigidbody2D rb;
    private EnemyHealth enemyHealth;

    private bool hasDetectedPlayer;
    private bool canAttack = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyHealth = GetComponent<EnemyHealth>();
        currentPatrolPoint = pointB;
        
        alertIcon.SetActive(false);
    }

    private void Update()
    {
        if (enemyHealth.GetIsDead()) return;
        CheckPlayerDetection();
    }

    private void FixedUpdate()
    {
        if (hasDetectedPlayer)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        Vector2 direction = (currentPatrolPoint.position - transform.position).normalized;

        rb.linearVelocity = new Vector2(direction.x * patrolSpeed, rb.linearVelocity.y);

        Flip(direction.x);

        if (Vector2.Distance(transform.position, currentPatrolPoint.position) < 0.2f)
        {
            if (currentPatrolPoint == pointA)
            {
                currentPatrolPoint = pointB;
            }
            else
            {
                currentPatrolPoint = pointA;
            }
        }
    }

    private void CheckPlayerDetection()
    {
        Vector2 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > detectionRange)
        {
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer.normalized, detectionRange, playerLayer | obstacleLayer);

        if (hit.collider.CompareTag("Player"))
        {
            if (!hasDetectedPlayer)
            {
                StartCoroutine(ShowAlertIcon());
            }

            hasDetectedPlayer = true;
        }
    }

    private void ChasePlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > stopDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * chaseSpeed, rb.linearVelocity.y);

            Flip(direction.x);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

            if (canAttack)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    private IEnumerator AttackCoroutine()
    {
        canAttack = false;

        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.3f);

        Collider2D[] targetsHit = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRadius,
            playerLayer);

        foreach (Collider2D target in targetsHit)
        {
            if (target.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(attackDamage);
            }
        }

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }

    private IEnumerator ShowAlertIcon()
    {
        alertIcon.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        alertIcon.SetActive(false);
    }

    private void Flip(float directionX)
    {
        if (directionX > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (directionX < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }
}
