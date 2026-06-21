using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float minAirControl;
    [SerializeField] private float maxAirControl;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpModifier;
    [SerializeField] private float ascendingJumpModifier;
    [SerializeField] private float descendingJumpModifier;
    [SerializeField] private float ascendingThreshold;
    [SerializeField] private Transform feet;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundDetectionRadius;

    [Header("Dash")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashCooldown;

    [Header("Debug")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool canDoubleJump;
    [SerializeField] private bool isDashing;
    [SerializeField] private bool controlsLocked;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    
    private int facingDirection = 1;
    private bool hasJumped;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        doubleJumpModifier = descendingJumpModifier;
    }

    private void Update()
    {
        if (controlsLocked)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }
        
        ReadInput();
        FlipCharacter();
        CheckIsGrounded();
        HandleJumpInput();
        HandleDashInput();
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            Move();
        }
    }

    private void ReadInput()
    {
        float hInput = Input.GetAxisRaw("Horizontal");
        moveDirection = new Vector2(hInput, 0f);
    }

    private void FlipCharacter()
    {
        if (moveDirection.x > 0)
        {
            facingDirection = 1;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (moveDirection.x < 0)
        {
            facingDirection = -1;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void Move()
    {
        float targetXVelocity = moveDirection.x * moveSpeed;

        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(targetXVelocity, rb.linearVelocity.y);
        }
        else
        {
            float currentAirControl = targetXVelocity != 0 ? maxAirControl : minAirControl;

            float newXVelocity = Mathf.MoveTowards(rb.linearVelocity.x, targetXVelocity, currentAirControl);

            rb.linearVelocity = new Vector2(newXVelocity, rb.linearVelocity.y);
        }
    }

    private void CheckIsGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(feet.position, groundDetectionRadius, groundLayer);

        canDoubleJump = !isGrounded && hasJumped;
    }

    private void HandleJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Space) && canDoubleJump)
        {
            DoubleJump();
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        hasJumped = true;
        
        AudioManager.Instance.PlayJumpSound();
    }

    private void DoubleJump()
    {
        if (rb.linearVelocity.y <= ascendingThreshold)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

            doubleJumpModifier = descendingJumpModifier;
        }
        else
        {
            doubleJumpModifier = ascendingJumpModifier;
        }

        rb.AddForce(Vector2.up * jumpForce * doubleJumpModifier, ForceMode2D.Impulse);

        doubleJumpModifier = descendingJumpModifier;
        hasJumped = false;
        canDoubleJump = false;
        
        AudioManager.Instance.PlayDoubleJumpSound();
    }

    private void HandleDashInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash();
        }
    }

    private void Dash()
    {
        StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true;

        rb.linearVelocity = new Vector2(transform.right.x * dashSpeed, rb.linearVelocity.y);

        yield return new WaitForSeconds(dashCooldown);

        isDashing = false;
    }
    
    public bool GetIsGrounded()
    {
        return isGrounded;
    }

    public bool GetIsDashing()
    {
        return isDashing;
    }
    
    public void SetControlsLocked(bool value)
    {
        controlsLocked = value;
    }
    
    public int GetFacingDirection()
    {
        return facingDirection;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(feet.position, groundDetectionRadius);
    }
}
