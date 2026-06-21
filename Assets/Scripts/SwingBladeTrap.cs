using System.Collections;
using UnityEngine;

public class SwingBladeTrap : MonoBehaviour
{
    [Header("Swing")]
    [SerializeField] private float maxAngle = 45f;
    [SerializeField] private float swingSpeed = 2f;

    [Header("Damage")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float damageCooldown = 0.5f;

    private Quaternion startRotation;
    private bool canDamage = true;

    private void Awake()
    {
        startRotation = transform.localRotation;
    }

    private void Update()
    {
        float angle = Mathf.Sin(Time.time * swingSpeed) * maxAngle;

        transform.localRotation = startRotation * Quaternion.Euler(0, 0, angle);
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
}
