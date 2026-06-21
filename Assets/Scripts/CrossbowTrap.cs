using System.Collections;
using UnityEngine;

public class CrossbowTrap : MonoBehaviour, IDamageable
{
    [Header("References")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject arrowPrefab;

    [Header("Detection")]
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRange = 8f;

    [Header("Shoot")]
    [SerializeField] private float shootCooldown = 2f;
    [SerializeField] private float shootDelay = 0.25f;
    [SerializeField] private int shootDirection = 1;

    [Header("Health")]
    [SerializeField] private int health = 1;
    [SerializeField] private float destroyDelay = 0.5f;

    private Animator animator;
    private bool canShoot = true;
    private bool isBroken;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (isBroken) return;

        CheckShoot();
    }

    private void CheckShoot()
    {
        if (!canShoot) return;
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            StartCoroutine(ShootCoroutine());
        }
    }

    private IEnumerator ShootCoroutine()
    {
        canShoot = false;

        if (animator != null)
        {
            animator.SetTrigger("Shoot");
        }

        yield return new WaitForSeconds(shootDelay);

        if (!isBroken)
        {
            GameObject arrow = Instantiate(
                arrowPrefab,
                shootPoint.position,
                Quaternion.identity);

            CrossbowArrow crossbowArrow = arrow.GetComponent<CrossbowArrow>();
            crossbowArrow.SetDirection(shootDirection);
        }

        yield return new WaitForSeconds(shootCooldown);

        canShoot = true;
    }

    public void TakeDamage(int damage)
    {
        if (isBroken) return;

        health -= damage;

        if (health <= 0)
        {
            Break();
        }
    }

    private void Break()
    {
        isBroken = true;
        canShoot = false;

        if (animator != null)
        {
            animator.SetTrigger("Break");
        }

        Destroy(gameObject, destroyDelay);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
