using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    float lerp = 40;
    GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }
    private void Update()
    {
        Vector2 playerPosition = player.transform.position;
        transform.right = Vector2.Lerp(transform.right, new Vector2(playerPosition.x - transform.position.x, playerPosition.y - transform.position.y), lerp * Time.deltaTime);
    }
}
