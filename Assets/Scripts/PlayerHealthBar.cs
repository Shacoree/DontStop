using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] public Slider healthSlider;
    [SerializeField] public Slider easeHealthSlider;

    private float maxHealth;
    private float health;

    private float lerpSpeed = 0.05f;

    void Start()
    {
        maxHealth = GameManager.gameManager.playerHealth.MaxHealth;
    }
    void Update()
    {
        health = GameManager.gameManager.playerHealth.Health;
        if(healthSlider.value != health)
            healthSlider.value = health;

        if(healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, lerpSpeed);
        }
    }
    private float GetPlayerMaxHealth()
    {
        return GameManager.gameManager.playerHealth.MaxHealth;
    }
}
