using System.Collections;
using Unity.Mathematics;
using UnityEngine;
public class Player : MonoBehaviour
{
    [SerializeField] public GameObject ballon;
    Rigidbody2D rb;
    float moveSpeed = 3;
    float stopSlideFactor = 1.05f;
    float jumpPower = 5;
    float ballonFallDownVelocity = -2;
    float dashPower = 8;
    float dashCoolDown = 0.8f;
    float wallRotationFactor = 15;
    bool allowJump = false;
    public static bool isGrounded = false;
    bool doubleJump = false;
    bool onWall = false;
    bool movingRight = true;
    int wallDirection;
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
        rb.gravityScale = 1;
        transform.rotation = Quaternion.identity;
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
            if (onWall)
            {
                doubleJump = false;
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
            if (onWall && !isGrounded)
            {
                wallJumpCouroutine = StartCoroutine(WallJump());
                moveCoroutine = null;
                yield break;
            }
            yield return null;
        }
    }
    IEnumerator Dash()
    {
        rb.gravityScale = 0;
        if (movingRight)
        {
            rb.velocity = new Vector2(dashPower, 0);
        }
        else
        {
            rb.velocity = new Vector2(-dashPower, 0);
        }
        StopCoroutine(moveCoroutine);
        moveCoroutine = null;
        yield return new WaitForSeconds(0.2f);
        rb.gravityScale = 1;
        moveCoroutine = StartCoroutine(Move());
        StartCoroutine(DashCoolDown());
        allowDash = false;
        yield return null;
    }
    IEnumerator WallJump()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine =null;
        }
        while (true)
        {
            doubleJump = false;
            if (rb.velocity.y < 0)
            {
                rb.gravityScale = 0;
                rb.velocity = new Vector2(rb.velocity.x, -2);
                transform.rotation = Quaternion.Euler(0, 0, wallDirection * wallRotationFactor);
            }
            if (isGrounded)
            {
                rb.velocity = new Vector2(0, 0);
                moveCoroutine = StartCoroutine(Move());
                wallJumpCouroutine = null;
                yield break;
            }
            if (Input.GetKeyDown(KeyCode.W) && rb.gravityScale != 1)
            {
                rb.gravityScale = 1;
                rb.AddForce(new Vector2(moveSpeed * -wallDirection, jumpPower) * 1.5f, ForceMode2D.Impulse);
                transform.rotation = Quaternion.identity;
                yield return new WaitForSeconds(0.3f);
                moveCoroutine = StartCoroutine(Move());
                wallJumpCouroutine = null;
                yield break;
            }
            yield return null;
        }
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
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Right Trigger" || collision.gameObject.tag == "Left Trigger")
        {
            onWall = true;
            if (collision.gameObject.tag == "Right Trigger")
            {
                wallDirection = 1;
            }
            else
            {
                wallDirection = -1;
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Right Trigger" || collision.gameObject.tag == "Left Trigger")
        {
            transform.rotation = quaternion.identity;
            onWall = false;
        }
    }
}
