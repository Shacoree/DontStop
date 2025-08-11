using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] public Transform[] spawnPoints;
    [SerializeField] private EnemyAi enemyprefab;
    [SerializeField] private SpecialEnemyAi specialEnemyPrefab;
    [SerializeField] private HookerEnemyAi hookerEnemyPrefab;
    private Transform children;

    [SerializeField] public float timeBetweenSpawns = 5.0f;
    [SerializeField] public float timeBetweenSpecialSpawns = 30.0f;
    [SerializeField] public float timeBetweenHookerSpawns = 60.0f;

    private float timeSinceLastSpawn = 0;
    private float timeSinceLastSpecialSpawn = 0;
    private float timeSinceLastHookerSpawn = 0;

    private IObjectPool<EnemyAi> enemyPool;
    private IObjectPool<SpecialEnemyAi> specialEnemyPool;
    private IObjectPool<HookerEnemyAi> hookerEnemyPool;

    void Start()
    {
        timeSinceLastSpawn = Time.time + timeBetweenSpawns;
        timeSinceLastSpecialSpawn = Time.time + timeBetweenSpecialSpawns;
        timeSinceLastHookerSpawn = Time.time + timeBetweenHookerSpawns;

        enemyPool = new ObjectPool<EnemyAi>(CreateEnemy, OnGet, OnRelease);
        specialEnemyPool = new ObjectPool<SpecialEnemyAi>(CreateSpecialEnemy, OnGet, OnRelease);
        hookerEnemyPool = new ObjectPool<HookerEnemyAi>(CreateHookerEnemy, OnGet, OnRelease);
    }
    void FixedUpdate()
    {
        if (Time.time > timeSinceLastSpawn)
        {
            int enemyCount = Random.Range(1, 100) / 20;
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


        if (Time.time > timeSinceLastHookerSpawn)
        {
            hookerEnemyPool.Get();

            timeSinceLastHookerSpawn = Time.time + timeBetweenHookerSpawns;
        }
        if (timeBetweenHookerSpawns > 2f)
        {
            timeBetweenHookerSpawns -= Time.fixedDeltaTime * 0.01f;
        }
    }

    private void OnGet(EnemyAi enemy)
    {
        enemy.GetComponent<CapsuleCollider>().enabled = true;

        children = enemy.GetComponent<Transform>();
        children.localPosition = Vector3.zero;
        children.localRotation = Quaternion.Euler(Vector3.zero);

        enemy.enemyHealth.Health = enemy.enemyHealth.MaxHealth;
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        enemy.transform.position = randomSpawnPoint.position;

        

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

    private HookerEnemyAi CreateHookerEnemy()
    {
        HookerEnemyAi enemy = Instantiate(hookerEnemyPrefab);
        enemy.SetPool(hookerEnemyPool);
        return enemy;
    }
}
