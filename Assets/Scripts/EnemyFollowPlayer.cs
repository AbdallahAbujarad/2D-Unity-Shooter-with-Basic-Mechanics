using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    Transform player;
    Rigidbody2D rb;
    float ySpeed = 1;
    float xSpeed = 2;
    bool isGrounded;
    private void Start()
    {
        player = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.gravityScale = 0;
    }
    void LateUpdate()
    {
        if (player == null) return;

        Vector2 newVelocity = Vector2.zero;

        if (Mathf.Abs(transform.position.y - player.position.y) < 0.05f && !Player.isGrounded)
        {
            newVelocity.y = 0;
        }
        else if (transform.position.y < player.position.y)
        {
            newVelocity.y = ySpeed;
        }
        else if (transform.position.y > player.position.y)
        {
            newVelocity.y = -ySpeed;
        }
        if (isGrounded)
        {
            if (transform.position.x < player.position.x)
            {
                newVelocity.x = xSpeed;
            }
            else if (transform.position.x > player.position.x)
            {
                newVelocity.x = -xSpeed;
            }
        }

        rb.velocity = newVelocity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            isGrounded = true;
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            isGrounded = false;
    }
}
