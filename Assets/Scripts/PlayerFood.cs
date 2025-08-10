using UnityEngine;

public class PlayerFood
{
    float maxAllowedFood = 80;
    float minAllowedFood = 20;
    float currentFood;
    float maxFood;

    public float MaxAllowedFood
    {
        get { return maxAllowedFood; }
        set { maxAllowedFood = value; }
    }
    public float MinAllowedFood
    {
        get { return minAllowedFood; }
        set { minAllowedFood = value; }
    }
    public float Food
    {
        get { return currentFood; }
        set { currentFood = value; }
    }
    public float MaxFood
    {
        get { return maxFood; }
        set { maxFood = value; }
    }
    public PlayerFood(float food, float maxFood)
    {
        currentFood = food;
        this.maxFood = maxFood;
    }
    public void LoseFood(float amount)
    {
        if (currentFood > 0)
        {
            currentFood -= amount;
        }
    }
    public void GiveFood(float amount)
    {
        if (currentFood < maxFood)
        {
            currentFood += amount;
        }
        if (currentFood > maxFood)
        {
            currentFood = maxFood;
        }
    }
}

