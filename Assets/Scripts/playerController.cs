using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float climbSpeed = 2.5f; // Reduced climb speed for slower climbing
    private float moveInput;
    private Animator animator;
    private bool facingRight = true;
    private bool isClimbing = false;
    private Rigidbody rb;
    private Collider ladder;

    void Start()
    {
        // Get the Animator and Rigidbody components attached to the player
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Get horizontal input (left and right movement)
        moveInput = Input.GetAxisRaw("Horizontal");

        // Apply movement to the player using Rigidbody velocity
        Vector3 horizontalVelocity = new Vector3(moveInput * moveSpeed, rb.linearVelocity.y, 0);
        rb.linearVelocity = horizontalVelocity;

        // Set isMoving based on whether there is horizontal input
        bool isMoving = Mathf.Abs(moveInput) > 0;
        animator.SetBool("isMoving", isMoving);

        // Flip the player to face the movement direction
        if (moveInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && facingRight)
        {
            Flip();
        }

        // Ladder climbing logic
        float verticalInput = Input.GetAxisRaw("Vertical");
        if (ladder != null && verticalInput > 0)
        {
            isClimbing = true;
            rb.useGravity = false;
            rb.linearVelocity = new Vector3(0, verticalInput * climbSpeed, 0);
        }
        else if (ladder != null && verticalInput == 0)
        {
            // Stop climbing when not holding the vertical input
            rb.linearVelocity = new Vector3(0, 0, 0);
        }
        else if (isClimbing && ladder == null)
        {
            isClimbing = false;
            rb.useGravity = true;
        }

        if (!isClimbing)
        {
            rb.useGravity = true;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            ladder = collision;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            ladder = null;
            rb.useGravity = true;
        }
    }
}