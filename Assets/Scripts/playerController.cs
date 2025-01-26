using UnityEngine;

public class PlayerController : MonoBehaviour, IDataPersistence
{
    public float moveSpeed = 5f;
    private float moveHorizontal;
    private float moveVertical;
    private Animator animator;
    private bool facingRight = true;
    private Rigidbody rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveHorizontal * moveSpeed, rb.linearVelocity.y, moveVertical * moveSpeed);
        rb.linearVelocity = movement;

        bool isMoving = Mathf.Abs(moveHorizontal) > 0 || Mathf.Abs(moveVertical) > 0;
        animator.SetBool("isMoving", isMoving);

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

    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = this.transform.position;
    }
}
