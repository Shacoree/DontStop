using UnityEngine;
using UnityEngine.UI;

public class PlayerFoodBar : MonoBehaviour
{
    [SerializeField] public Slider foodSlider;
    private float maxFood;
    private float food;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxFood = GameManager.gameManager.playerFood.MaxFood;
    }

    // Update is called once per frame
    void Update()
    {
        food = GameManager.gameManager.playerFood.Food;
        if (foodSlider.value != food)
            foodSlider.value = food;
    }
}
