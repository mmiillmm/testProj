using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IDataPersistence
{
    public float moveSpeed = 5f;
    private float moveHorizontal;
    private float moveVertical;
    private Animator animator;
    private bool facingRight = true;
    private Rigidbody rb;
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float fireballSpeed = 10f;

    [Header("Spawn Settings")]
    public Vector3 defaultSpawnPosition = new Vector3(0f, 1f, 0f);

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        if (transform.position.y < -50f)
        {
            transform.position = defaultSpawnPosition;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootFireball();
        }

        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveHorizontal * moveSpeed, rb.linearVelocity.y, moveVertical * moveSpeed);
        rb.linearVelocity = movement;

        animator.SetBool("isMoving", Mathf.Abs(moveHorizontal) > 0 || Mathf.Abs(moveVertical) > 0);

        if (moveHorizontal > 0 && !facingRight)
            Flip();
        else if (moveHorizontal < 0 && facingRight)
            Flip();
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void ShootFireball()
    {
        Vector3 direction = facingRight ? Vector3.right : Vector3.left;
        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = fireball.GetComponent<Rigidbody>();
        rb.linearVelocity = direction * fireballSpeed;
    }

    public void LoadData(GameData data)
    {
        if (SceneManager.GetActiveScene().name == "SnowTest")
        {
            if (data != null && data.snowTestPlayerPosition.y > -100f)
            {
                transform.position = data.snowTestPlayerPosition;
            }
            else
            {
                Debug.LogWarning("def spawn, nincs savelt hely");
                transform.position = defaultSpawnPosition;
            }
        }
    }


    public void SaveData(ref GameData data)
    {
        if (SceneManager.GetActiveScene().name == "SnowTest")
        {
            data.snowTestPlayerPosition = transform.position;
        }
    }
}
