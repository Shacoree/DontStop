using System;
using UnityEngine;
using UnityEngine.Pool;

public class FoodManager : MonoBehaviour
{

    [SerializeField] private Transform[] spawnPoints;
    private static float timeBetweenSpawns = 30;

    public static float TimeBetweenSpawns
    {
        get { return timeBetweenSpawns; }
        set { timeBetweenSpawns = value; }
    }
    private static float timeSinceLastSpawn = 5;

    public static float TimeSinceLastSpawn
    {
        get { return timeSinceLastSpawn; }
        set { timeSinceLastSpawn = value; }
    }

    private static int foodSpawnedCount;
    private static int maxFoodSpawnedCount = 1;

    public static int FoodSpawnedCount 
    {  
        get { return foodSpawnedCount; }
        set { foodSpawnedCount = value; }
    }

    [SerializeField] private Food foodPrefab;
    private IObjectPool<Food> foodPool;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        foodPool = new ObjectPool<Food>(CreateFood, OnGet, OnRelease);
        foodSpawnedCount = 0;
    }
    private Food CreateFood()
    {
        Food food =  Instantiate(foodPrefab);
        food.SetPool(foodPool);
        return food;
    }
    private void  OnGet(Food food)
    {
        food.gameObject.SetActive(true);

        Transform randomSpawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        food.transform.position = randomSpawnPoint.position;
    }

    private void OnRelease(Food food)
    {
        food.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time > timeSinceLastSpawn && foodSpawnedCount < maxFoodSpawnedCount)
        {
            foodSpawnedCount++;
            foodPool.Get();
        }
    }
}
