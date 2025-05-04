using UnityEngine;

public class EnemySpawnerSpawner : MonoBehaviour
{
    public GameObject enemySpawnerPrefab;
    void Start()
    {
        InvokeRepeating(nameof(Spawn), 0, 15);
    }
    void Spawn()
    {
        if (enemySpawnerPrefab != null)
        {
            Instantiate(enemySpawnerPrefab,new Vector2(Random.Range(-7.0f,7.0f),Random.Range(-3.0f,3.0f)),Quaternion.identity);
        }
    }
}
