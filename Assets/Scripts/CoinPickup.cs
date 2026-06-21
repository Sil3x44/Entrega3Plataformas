using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float collectDistance = 0.15f;

    private Transform player;
    private bool isFollowingPlayer;

    private void Update()
    {
        if (!isFollowingPlayer || player == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, player.position) <= collectDistance)
        {
            GameManager.Instance.AddCoins(1);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            isFollowingPlayer = true;
        }
    }
}
