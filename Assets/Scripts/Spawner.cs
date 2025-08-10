using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] public Transform[] spawnPoints;
    [SerializeField] public float timeBetweenSpawns = 5.0f;
    [SerializeField] public float timeBetweenSpecialSpawns = 30.0f;
    private Transform children;

    private float timeSinceLastSpawn = 5;
    private float timeSinceLastSpecialSpawn = 30;

    [SerializeField] private EnemyAi enemyprefab;
    [SerializeField] private SpecialEnemyAi specialEnemyPrefab;
    private IObjectPool<EnemyAi> enemyPool;
    private IObjectPool<SpecialEnemyAi> specialEnemyPool;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeSinceLastSpawn = 5;
        timeSinceLastSpecialSpawn = 30;
        enemyPool = new ObjectPool<EnemyAi>(CreateEnemy, OnGet, OnRelease);
        specialEnemyPool = new ObjectPool<SpecialEnemyAi>(CreateSpecialEnemy, OnGet, OnRelease);
    }
    private void OnGet(EnemyAi enemy)
    {
        enemy.GetComponent<CapsuleCollider>().enabled = true;

        enemy.enemyHealth.Health = enemy.enemyHealth.MaxHealth;
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        enemy.transform.position = randomSpawnPoint.position;

        children = enemy.GetComponent<Transform>();
        children.localPosition = Vector3.zero;
        children.localRotation = Quaternion.Euler(Vector3.zero);

        enemy.IsDead = false;
        enemy.HasStartedJump = false;
        enemy.gameObject.SetActive(true);
    }
    private void OnRelease(EnemyAi enemy)
    {
        enemy.gameObject.SetActive(false);  
    }
    private EnemyAi CreateEnemy()
    {
        EnemyAi enemy = Instantiate(enemyprefab);
        enemy.SetPool(enemyPool);
        return enemy;
    }
    private SpecialEnemyAi CreateSpecialEnemy()
    {
        SpecialEnemyAi enemy = Instantiate(specialEnemyPrefab);
        enemy.SetPool(specialEnemyPool);
        return enemy;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time > timeSinceLastSpawn)
        {
            int enemyCount = Random.Range(1, 100)/20;
            for (int i = 0; i < enemyCount; i++)
            {
                enemyPool.Get();
            }
            timeSinceLastSpawn = Time.time + timeBetweenSpawns;
        }
        if (timeBetweenSpawns > 0.5f)
        {
            timeBetweenSpawns -= Time.fixedDeltaTime * 0.01f;
        }

        if (Time.time > timeSinceLastSpecialSpawn)
        {
            specialEnemyPool.Get();
            
            timeSinceLastSpecialSpawn = Time.time + timeBetweenSpecialSpawns;
        }
        if (timeBetweenSpecialSpawns > 0.5f)
        {
            timeBetweenSpecialSpawns -= Time.fixedDeltaTime * 0.01f;
        }
    }
}
