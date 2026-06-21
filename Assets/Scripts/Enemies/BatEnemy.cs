using System.Collections;
using UnityEngine;

public class BatEnemy : MonoBehaviour
{
    private enum BatState
    {
        Hanging,
        FlyingAbovePlayer,
        MovingToChargePosition,
        Charging,
        ReturningAbovePlayer
    }

    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask playerLayer;

    [Header("Detection")]
    [SerializeField] private float detectionRange = 7f;
    [SerializeField] private float alertDuration = 0.5f;
    [SerializeField] private float takeOffDelay = 0.3f;

    [Header("Flying")]
    [SerializeField] private float flySpeed = 3f;
    [SerializeField] private float heightAbovePlayer = 3f;
    [SerializeField] private float sideMoveDistance = 2f;
    [SerializeField] private float flySideToSideTime = 1.5f;

    [Header("Charge")]
    [SerializeField] private float chargeOffsetX = 3f;
    [SerializeField] private float chargeSpeed = 10f;
    [SerializeField] private float chargeDuration = 0.6f;
    [SerializeField] private float waitBetweenAttacks = 1f;
    [SerializeField] private int damage = 1;

    [Header("Feedback")]
    [SerializeField] private GameObject alertIcon;

    private Rigidbody2D rb;
    private EnemyHealth enemyHealth;
    private Animator animator;

    private BatState currentState;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    private int facingDirection = 1;
    private bool hasDamagedPlayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyHealth = GetComponent<EnemyHealth>();
        animator = GetComponentInChildren<Animator>();

        rb.gravityScale = 0f;

        startPosition = transform.position;
        currentState = BatState.Hanging;

        if (alertIcon != null)
        {
            alertIcon.SetActive(false);
        }
    }

    private void Update()
    {
        if (enemyHealth.GetIsDead()) return;

        if (currentState == BatState.Hanging)
        {
            CheckPlayerDetection();
        }
    }

    private void FixedUpdate()
    {
        if (enemyHealth.GetIsDead()) return;

        if (currentState == BatState.MovingToChargePosition ||
            currentState == BatState.ReturningAbovePlayer)
        {
            MoveToTargetPosition();
        }
    }

    private void CheckPlayerDetection()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && currentState == BatState.Hanging)
        {
            StartCoroutine(DetectionSequence());
        }
    }

    private IEnumerator DetectionSequence()
    {
        currentState = BatState.FlyingAbovePlayer;
        
        alertIcon.SetActive(true);

        yield return new WaitForSeconds(alertDuration);
        
        alertIcon.SetActive(false);
        
        yield return new WaitForSeconds(takeOffDelay);
        
        animator.SetBool("IsFlying", true);
        
        StartCoroutine(FlySideToSideThenCharge());
    }

    private IEnumerator FlySideToSideThenCharge()
    {
        currentState = BatState.FlyingAbovePlayer;

        float timer = 0f;

        while (timer < flySideToSideTime)
        {
            if (enemyHealth.GetIsDead()) yield break;

            float sideOffset = Mathf.Sin(Time.time * 4f) * sideMoveDistance;

            Vector3 desiredPosition = new Vector3(
                player.position.x + sideOffset,
                player.position.y + heightAbovePlayer,
                transform.position.z);

            transform.position = Vector3.MoveTowards(
                transform.position,
                desiredPosition,
                flySpeed * Time.deltaTime);

            timer += Time.deltaTime;

            yield return null;
        }

        PrepareCharge();
    }

    private void PrepareCharge()
    {
        facingDirection = player.position.x > transform.position.x ? 1 : -1;

        targetPosition = new Vector3(
            player.position.x - facingDirection * chargeOffsetX,
            player.position.y,
            transform.position.z);

        Flip(facingDirection);

        currentState = BatState.MovingToChargePosition;
    }

    private void MoveToTargetPosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, flySpeed * Time.fixedDeltaTime);

        if (Vector3.Distance(transform.position, targetPosition) <= 0.1f)
        {
            if (currentState == BatState.MovingToChargePosition)
            {
                StartCoroutine(ChargeCoroutine());
            }
            else if (currentState == BatState.ReturningAbovePlayer)
            {
                StartCoroutine(WaitAndRepeat());
            }
        }
    }

    private IEnumerator ChargeCoroutine()
    {
        currentState = BatState.Charging;
        hasDamagedPlayer = false;

        animator.SetTrigger("Attack");

        rb.linearVelocity = new Vector2(facingDirection * chargeSpeed, 0f);

        yield return new WaitForSeconds(chargeDuration);

        rb.linearVelocity = Vector2.zero;

        targetPosition = new Vector3(player.position.x, player.position.y + heightAbovePlayer, transform.position.z);

        currentState = BatState.ReturningAbovePlayer;
    }

    private IEnumerator WaitAndRepeat()
    {
        currentState = BatState.FlyingAbovePlayer;

        yield return new WaitForSeconds(waitBetweenAttacks);

        StartCoroutine(FlySideToSideThenCharge());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (currentState != BatState.Charging) return;
        if (hasDamagedPlayer) return;

        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
                hasDamagedPlayer = true;
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
