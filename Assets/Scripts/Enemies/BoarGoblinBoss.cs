using System.Collections;
using UnityEngine;

public class BoarGoblinBoss : MonoBehaviour
{
    private enum BossState
    {
        Patrol,
        Chase,
        SpearAttack,
        Charge,
        Recover
    }

    [Header("Scene References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private Transform spearAttackPoint;
    [SerializeField] private GameObject alertIcon;
    [SerializeField] private LayerMask playerLayer;

    [Header("Patrol")]
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float pointDetectionDistance = 0.2f;

    [Header("Detection")]
    [SerializeField] private float detectionRange = 8f;

    [Header("Chase")]
    [SerializeField] private float chaseSpeed = 3f;
    [SerializeField] private float spearRange = 1.5f;

    [Header("Spear Attack")]
    [SerializeField] private float spearAttackRadius = 0.8f;
    [SerializeField] private int spearDamage = 1;
    [SerializeField] private float spearDamageDelay = 0.25f;
    [SerializeField] private float spearCooldown = 1f;

    [Header("Charge")]
    [SerializeField] private float chargeMinDistance = 3f;
    [SerializeField] private float chargeMaxDistance = 6f;
    [SerializeField] private float chargeSpeed = 9f;
    [SerializeField] private float chargeDuration = 0.8f;
    [SerializeField] private int chargeDamage = 2;
    [SerializeField] private float recoverTime = 0.8f;

    private Rigidbody2D rb;
    private EnemyHealth enemyHealth;
    private Animator animator;

    private BossState currentState;
    private int currentPatrolIndex;
    private int facingDirection = 1;

    private bool hasDetectedPlayer;
    private bool canAct = true;
    private bool hasDamagedPlayerInCharge;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyHealth = GetComponent<EnemyHealth>();
        animator = GetComponentInChildren<Animator>();

        currentState = BossState.Patrol;

        alertIcon.SetActive(false);

        SelectRandomPatrolPoint();
    }

    private void Update()
    {
        if (enemyHealth.GetIsDead()) return;

        CheckDetection();
    }

    private void FixedUpdate()
    {
        if (enemyHealth.GetIsDead()) return;

        if (currentState == BossState.Patrol)
        {
            Patrol();
        }
        else if (currentState == BossState.Chase)
        {
            ChaseAndDecideAttack();
        }
    }

    private void CheckDetection()
    {
        if (hasDetectedPlayer) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            hasDetectedPlayer = true;
            currentState = BossState.Chase;
            StartCoroutine(ShowAlertIcon());
        }
    }

    private void Patrol()
    {
        Transform targetPoint = patrolPoints[currentPatrolIndex];

        Vector2 direction = (targetPoint.position - transform.position).normalized;

        rb.linearVelocity = new Vector2(direction.x * patrolSpeed, rb.linearVelocity.y);

        Flip(direction.x);

        if (Vector2.Distance(transform.position, targetPoint.position) <= pointDetectionDistance)
        {
            SelectRandomPatrolPoint();
        }
    }

    private void SelectRandomPatrolPoint()
    {

        int newIndex = Random.Range(0, patrolPoints.Length);

        while (newIndex == currentPatrolIndex)
        {
            newIndex = Random.Range(0, patrolPoints.Length);
        }

        currentPatrolIndex = newIndex;
    }

    private void ChaseAndDecideAttack()
    {
        if (!canAct) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        float directionToPlayerX = player.position.x - transform.position.x;

        Flip(directionToPlayerX);

        if (distanceToPlayer >= chargeMinDistance && distanceToPlayer <= chargeMaxDistance)
        {
            StartCoroutine(ChargeCoroutine());
        }
        else if (distanceToPlayer <= spearRange)
        {
            StartCoroutine(SpearAttackCoroutine());
        }
        else
        {
            Vector2 direction = (player.position - transform.position).normalized;

            rb.linearVelocity = new Vector2(direction.x * chaseSpeed, rb.linearVelocity.y);
        }
    }

    private IEnumerator SpearAttackCoroutine()
    {
        currentState = BossState.SpearAttack;
        canAct = false;

        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(spearDamageDelay);

        Collider2D[] targetsHit = Physics2D.OverlapCircleAll(spearAttackPoint.position, spearAttackRadius, playerLayer);

        foreach (Collider2D target in targetsHit)
        {
            if (target.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(spearDamage);
            }
        }

        yield return new WaitForSeconds(spearCooldown);

        currentState = BossState.Chase;
        canAct = true;
    }

    private IEnumerator ChargeCoroutine()
    {
        currentState = BossState.Charge;
        canAct = false;
        hasDamagedPlayerInCharge = false;

        animator.SetBool("Charge", true);

        rb.linearVelocity = new Vector2(facingDirection * chargeSpeed, rb.linearVelocity.y);

        yield return new WaitForSeconds(chargeDuration);

        animator.SetBool("Charge", false);

        rb.linearVelocity = Vector2.zero;

        currentState = BossState.Recover;

        yield return new WaitForSeconds(recoverTime);

        currentState = BossState.Chase;
        canAct = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (currentState != BossState.Charge) return;
        if (hasDamagedPlayerInCharge) return;

        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(chargeDamage);
                hasDamagedPlayerInCharge = true;
            }
        }
    }

    private void Flip(float directionX)
    {
        if (directionX > 0)
        {
            facingDirection = 1;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (directionX < 0)
        {
            facingDirection = -1;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private IEnumerator ShowAlertIcon()
    {
        if (alertIcon == null) yield break;

        alertIcon.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        alertIcon.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        if (spearAttackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(spearAttackPoint.position, spearAttackRadius);
        }
    }
}
