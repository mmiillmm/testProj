using UnityEngine;

public class LadderClimb : MonoBehaviour
{
    public float climbSpeed = 5f;
    private bool isClimbing = false;
    private Rigidbody2D rb;
    private Collider2D ladder;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (ladder != null && Input.GetAxis("Vertical") != 0)
        {
            isClimbing = true;
            rb.gravityScale = 0;
        }
        else if (isClimbing && ladder == null)
        {
            isClimbing = false;
            rb.gravityScale = 1;
        }

        if (isClimbing)
        {
            float verticalInput = Input.GetAxis("Vertical");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, verticalInput * climbSpeed);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            ladder = collision;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            ladder = null;
            rb.gravityScale = 1;
        }
    }
}
