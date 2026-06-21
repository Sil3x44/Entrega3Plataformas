using System.Collections;
using UnityEngine;

public class CeilingSpikesTrap : MonoBehaviour
{
    private enum TrapState
    {
        Waiting,
        Falling,
        Rising
    }

    [Header("Detection")]
    [SerializeField] private Transform player;
    [SerializeField] private float detectionWidth = 2f;
    [SerializeField] private float detectionHeight = 5f;

    [Header("Movement")]
    [SerializeField] private float fallDistance = 4f;
    [SerializeField] private float fallSpeed = 12f;
    [SerializeField] private float riseSpeed = 3f;
    [SerializeField] private float waitBeforeRise = 0.4f;

    [Header("Damage")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float damageCooldown = 0.5f;

    private Vector3 startPosition;
    private Vector3 bottomPosition;
    private TrapState currentState;

    private bool canDamage = true;

    private void Awake()
    {
        startPosition = transform.position;
        bottomPosition = startPosition + Vector3.down * fallDistance;
        currentState = TrapState.Waiting;
    }

    private void Update()
    {
        if (currentState == TrapState.Waiting)
        {
            CheckPlayerBelow();
        }
        else if (currentState == TrapState.Falling)
        {
            Fall();
        }
        else if (currentState == TrapState.Rising)
        {
            Rise();
        }
    }

    private void CheckPlayerBelow()
    {
        if (player == null) return;

        bool playerInsideX =
            player.position.x > transform.position.x - detectionWidth / 2f &&
            player.position.x < transform.position.x + detectionWidth / 2f;

        bool playerBelow =
            player.position.y < transform.position.y &&
            player.position.y > transform.position.y - detectionHeight;

        if (playerInsideX && playerBelow)
        {
            currentState = TrapState.Falling;
        }
    }

    private void Fall()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            bottomPosition,
            fallSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, bottomPosition) <= 0.05f)
        {
            StartCoroutine(WaitAndRiseCoroutine());
        }
    }

    private IEnumerator WaitAndRiseCoroutine()
    {
        currentState = TrapState.Waiting;

        yield return new WaitForSeconds(waitBeforeRise);

        currentState = TrapState.Rising;
    }

    private void Rise()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            startPosition,
            riseSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, startPosition) <= 0.05f)
        {
            currentState = TrapState.Waiting;
        }
    }

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

    private IEnumerator DamageCooldownCoroutine()
    {
        canDamage = false;

        yield return new WaitForSeconds(damageCooldown);

        canDamage = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 center = transform.position + Vector3.down * detectionHeight / 2f;
        Vector3 size = new Vector3(detectionWidth, detectionHeight, 0f);

        Gizmos.DrawWireCube(center, size);
    }
}
