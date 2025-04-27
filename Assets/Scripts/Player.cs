using System.Collections;
using UnityEngine;
public class Player : MonoBehaviour
{
    [SerializeField] public GameObject ballon;
    Rigidbody2D rb;
    float moveSpeed = 3;
    float stopSlideFactor = 1.05f;
    float jumpPower = 4;
    float ballonFallDownVelocity = -2;
    float dashPower = 8;
    float dashCoolDown = 0.8f;
    bool allowJump = false;
    bool isGrounded = false;
    bool doubleJump = false;
    bool movingRight = true;
    Coroutine moveCoroutine;
    Coroutine dashCoroutine;
    Coroutine wallJumpCouroutine;
    bool allowDash = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        moveCoroutine = StartCoroutine(Move());
        ballon.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && dashCoroutine == null && allowDash)
        {
            dashCoroutine = StartCoroutine(Dash());
        }
    }
    IEnumerator DashCoolDown()
    {
        yield return new WaitForSeconds(dashCoolDown);
        allowDash = true;
    }
    IEnumerator Move()
    {
        if (dashCoroutine != null)
        {
            dashCoroutine = null;
        }
        while (true)
        {
            if (Input.GetKey(KeyCode.D))
            {
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                movingRight = true;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
                movingRight = false;
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x / stopSlideFactor, rb.velocity.y);
            }
            allowJump = isGrounded || doubleJump;
            if (Input.GetKeyDown(KeyCode.W) && allowJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                if (doubleJump)
                {
                    doubleJump = false;
                }
            }
            if (Input.GetKey(KeyCode.E) && !isGrounded)
            {
                ballon.SetActive(true);
                if (rb.velocity.y < 0)
                {
                    rb.gravityScale = 0;
                    rb.velocity = new Vector2(rb.velocity.x, ballonFallDownVelocity);
                }
                else
                {
                    rb.gravityScale = 1;
                }
            }
            else
            {
                rb.gravityScale = 1;
                ballon.SetActive(false);
            }
            yield return null;
        }
    }
    IEnumerator Dash()
    {
        rb.gravityScale = 0;
        if (movingRight)
        {
            rb.velocity = new Vector2(dashPower,0);
        }
        else
        {
            rb.velocity = new Vector2(-dashPower,0);
        }
        StopCoroutine(moveCoroutine);
        yield return new WaitForSeconds(0.2f);
        rb.gravityScale = 1;
        moveCoroutine = StartCoroutine(Move());
        StartCoroutine(DashCoolDown());
        allowDash = false;
        yield return null;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            doubleJump = false;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
            doubleJump = true;
        }
    }
}
