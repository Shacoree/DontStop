using UnityEngine;

public class UnitHealth
{
    float currentHealth;
    float maxHealth;
    public float Health
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }
    public float MaxHealth
    { 
        get { return maxHealth; } 
        set { maxHealth = value; } 
    }
    public UnitHealth(float health, float maxHealth)
    {
        currentHealth = health;
        this.maxHealth = maxHealth;
    }
    public void TakeDamage(float damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
        }
    }
    public void Heal(float amount)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += amount;
        }
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
