using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnRate;
    [SerializeField] private GridRing spawnRing;
    void Awake()
    {
        GridEntityManager.OnGridEntityPlaced += StartSpawningEnemies;
    }

    private void StartSpawningEnemies(Vector2Int _, GridEntity __)
    {
        GridEntityManager.OnGridEntityPlaced -= StartSpawningEnemies;
        InvokeRepeating(nameof(SpawnEnemy), 0, spawnRate);
    }

    private void SpawnEnemy()
    {
        var enemy = Instantiate(enemyPrefab).GetComponent<Enemy>();
        enemy.InitializeEnemy(spawnRing.GetRandomPoint());
    }
}
