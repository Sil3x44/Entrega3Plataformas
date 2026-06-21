using UnityEngine;

public class SkeletonAnimations : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody2D>();
    }

    private void Update()
    {
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
    }
}
