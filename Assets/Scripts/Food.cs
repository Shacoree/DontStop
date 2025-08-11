using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Food : MonoBehaviour
{

    [SerializeField] private float foodGiveAmount = 30.0f;
    private IObjectPool<Food> foodPool;

    public void ReleaseEnemy()
    {
        foodPool.Release(this);
    }
    public void SetPool(IObjectPool<Food> pool)
    {
        foodPool = pool;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other == null || other.IsDestroyed()) return;

        if (other.CompareTag("Player"))
        {

            GameManager.gameManager.playerFood.GiveFood(foodGiveAmount);

            FoodManager.FoodSpawnedCount--;
            FoodManager.TimeSinceLastSpawn = Time.time + FoodManager.TimeBetweenSpawns;
            foodPool.Release(this);
        }
    }
}
