using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    private Slider healthSlider, easeHealthSlider;
    private EnemyAi enemy;

    private float maxHealth;
    private float health;

    private float lerpSpeed = 0.05f;

    void Awake()
    {
        healthSlider = GetComponentsInChildren<Slider>()[0];
        easeHealthSlider = GetComponentsInChildren<Slider>()[1];
        enemy = GetComponentInParent<EnemyAi>();

        maxHealth = enemy.enemyHealth.MaxHealth;

        healthSlider.maxValue = maxHealth;
        easeHealthSlider.maxValue = maxHealth;
    }
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
