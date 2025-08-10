using UnityEngine;

public class PlayerRage
{
    float currentRage;
    float maxRage;
    public float Rage
    {
        get { return currentRage; }
        set { currentRage = value; }
    }
    public float MaxRage
    {
        get { return maxRage; }
        set { maxRage = value; }
    }
    public PlayerRage(float rage, float maxRage)
    {
        currentRage = rage;
        this.maxRage = maxRage;
    }
    public void LoseRage(float amount)
    {
        if (currentRage > 0)
        {
            currentRage -= amount;
        }
    }
    public void GiveRage(float amount)
    {
        if (currentRage < maxRage)
        {
            currentRage += amount;
        }
        if (currentRage > maxRage)
        {
            currentRage = maxRage;
        }
    }
}

