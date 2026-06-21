using UnityEngine;

public class CrossbowArrow : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifeTime = 3f;

    private int directionX = 1;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += Vector3.right * directionX * moveSpeed * Time.deltaTime;
    }

    public void SetDirection(int newDirectionX)
    {
        directionX = newDirectionX;

        if (directionX > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
