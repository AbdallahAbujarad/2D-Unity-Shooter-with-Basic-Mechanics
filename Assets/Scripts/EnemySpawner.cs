using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    private void Start()
    {
        InvokeRepeating(nameof(Spawn), 0, 5);
    }
    void Spawn()
    {
        if (enemyPrefab != null)
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        }
    }
}
