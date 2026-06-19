using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float airControl;
    private float minAirControl = 0.2f;
    private float maxAirControl = 1f;

    private Vector2 moveDirection;
    
    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpModifier;
    [SerializeField] private float ascendingJumpModifier;
    [SerializeField] private float descendingJumpModifier;
    [SerializeField] private Transform feet;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundDetectionRadius = 0.25f;
    
    
    [SerializeField] private bool isGrounded;
    private bool hasJumped;
    [SerializeField] private bool canDoubleJump;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        doubleJumpModifier = descendingJumpModifier;
    }

    private void Update()
    {
        float hInput = Input.GetAxisRaw("Horizontal");
        moveDirection = new Vector2(hInput, rb.linearVelocity.y);

        if (hInput > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (hInput < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        CheckIsGrounded();
        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Space) && canDoubleJump)
        {
            DoubleJump();
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float targetXVelocity = moveDirection.x * moveSpeed;
        airControl = targetXVelocity != 0 ? airControl = maxAirControl : airControl = minAirControl;

        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(targetXVelocity, rb.linearVelocity.y);
        }
        else
        {
            float newXVelocity = Mathf.MoveTowards(rb.linearVelocity.x, targetXVelocity, airControl);

            rb.linearVelocity = new Vector2(newXVelocity, rb.linearVelocity.y);
        }
        
    }

    private void CheckIsGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(feet.position, groundDetectionRadius, groundLayer);

        canDoubleJump = (!isGrounded && hasJumped) ? canDoubleJump = true : canDoubleJump = false;
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        hasJumped = true;
    }

    private void DoubleJump()
    {
        if (rb.linearVelocity.y <= 2)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            doubleJumpModifier = descendingJumpModifier;
        }
        else if (rb.linearVelocity.y > 2)
        {
            doubleJumpModifier = ascendingJumpModifier;
        }
        
        rb.AddForce(Vector2.up * jumpForce * doubleJumpModifier, ForceMode2D.Impulse);
        doubleJumpModifier = descendingJumpModifier;
        hasJumped = false;
        canDoubleJump = false;
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(feet.position, groundDetectionRadius);
    }
}
