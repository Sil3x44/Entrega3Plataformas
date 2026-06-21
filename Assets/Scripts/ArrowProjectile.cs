using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private GameObject collectEffectPrefab;

    private int damage;
    private float directionX = 1f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector2.right * directionX * moveSpeed * Time.deltaTime);
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    public void SetDirection(float newDirectionX)
    {
        directionX = newDirectionX;

        if (directionX < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
                Instantiate(collectEffectPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
