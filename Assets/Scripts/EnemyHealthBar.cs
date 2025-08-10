using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    private Slider healthSlider;
    private Slider easeHealthSlider;
    private EnemyAi enemy;
    private float maxHealth;
    private float health;
    private float lerpSpeed = 0.05f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        healthSlider = GetComponentsInChildren<Slider>()[0];
        easeHealthSlider = GetComponentsInChildren<Slider>()[1];
        enemy = GetComponentInParent<EnemyAi>();
        maxHealth = enemy.enemyHealth.MaxHealth;
        healthSlider.maxValue = maxHealth;
        easeHealthSlider.maxValue = maxHealth;
    }
    // Update is called once per frame
    void Update()
    {
        health = enemy.enemyHealth.Health;
        if (healthSlider.value != health)
            healthSlider.value = health;

        if (healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, lerpSpeed);
        }
    }
}
