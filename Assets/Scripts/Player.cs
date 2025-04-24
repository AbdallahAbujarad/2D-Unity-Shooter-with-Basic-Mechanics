using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    float moveSpeed = 3;
    float stopSlideFactor = 1.05f;
    float jumpPower = 7;
    bool allowJump = false;
    bool isGrounded = false;
    bool doubleJump = false;
    Coroutine moveCoroutine;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        moveCoroutine = StartCoroutine(Move());
    }
    void Update()
    {

    }
    IEnumerator Move()
    {
        while (true)
        {
            if (Input.GetKey(KeyCode.D))
            {
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x / stopSlideFactor, rb.velocity.y);
            }

            allowJump = isGrounded || doubleJump;
            if (Input.GetKeyDown(KeyCode.W) && allowJump)
            {
                rb.velocity = new Vector2(rb.velocity.x,jumpPower);
                if (doubleJump)
                {
                    doubleJump = false;
                }
            }
            yield return null;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded =  true;
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
