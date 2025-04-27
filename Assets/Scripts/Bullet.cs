using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    float powerX = 10;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.AddForce(transform.right * powerX, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
