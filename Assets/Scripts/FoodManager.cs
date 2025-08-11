using System;
using UnityEngine;
using UnityEngine.Pool;

public class FoodManager : MonoBehaviour
{

    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Food foodPrefab;

    private IObjectPool<Food> foodPool;

    private static float timeBetweenSpawns = 30;
    private static float timeSinceLastSpawn = 5;

    private static int foodSpawnedCount;
    private static int maxFoodSpawnedCount = 1;

    public static float TimeBetweenSpawns
    {
        get { return timeBetweenSpawns; }
        set { timeBetweenSpawns = value; }
    }
    public static float TimeSinceLastSpawn
    {
        get { return timeSinceLastSpawn; }
        set { timeSinceLastSpawn = value; }
    }
    public static int FoodSpawnedCount 
    {  
        get { return foodSpawnedCount; }
        set { foodSpawnedCount = value; }
    }

    void Awake()
    {
        foodPool = new ObjectPool<Food>(CreateFood, OnGet, OnRelease);
        foodSpawnedCount = 0;
    }
    void FixedUpdate()
    {
        if (Time.time > timeSinceLastSpawn && foodSpawnedCount < maxFoodSpawnedCount)
        {
            foodSpawnedCount++;
            foodPool.Get();
        }
    }
    private Food CreateFood()
    {
        Food food =  Instantiate(foodPrefab);
        food.SetPool(foodPool);
        return food;
    }
    private void OnGet(Food food)
    {
        food.gameObject.SetActive(true);

        Transform randomSpawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        food.transform.position = randomSpawnPoint.position;
    }

    private void OnRelease(Food food)
    {
        food.gameObject.SetActive(false);
    }

    
}
