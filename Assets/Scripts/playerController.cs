using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private float moveHorizontal;
    private float moveVertical;
    private Animator animator;
    private bool facingRight = true;
    private Rigidbody rb;

    void Start()
    {
        // Get the Animator and Rigidbody components attached to the player
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Get input for horizontal (left/right) and vertical (forward/backward) movement
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        // Apply movement to the player using Rigidbody velocity
        Vector3 movement = new Vector3(moveHorizontal * moveSpeed, rb.linearVelocity.y, moveVertical * moveSpeed);
        rb.linearVelocity = movement;

        // Set isMoving based on whether there is any movement input
        bool isMoving = Mathf.Abs(moveHorizontal) > 0 || Mathf.Abs(moveVertical) > 0;
        animator.SetBool("isMoving", isMoving);

        // Flip the player to face the movement direction (horizontal only)
        if (moveHorizontal > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveHorizontal < 0 && facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
