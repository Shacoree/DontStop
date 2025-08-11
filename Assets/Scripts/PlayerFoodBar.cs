using UnityEngine;
using UnityEngine.UI;

public class PlayerFoodBar : MonoBehaviour
{
    [SerializeField] public Slider foodSlider;

    private float maxFood;
    private float food;

    void Start()
    {
        maxFood = GameManager.gameManager.playerFood.MaxFood;
    }

    void Update()
    {
        food = GameManager.gameManager.playerFood.Food;
        if (foodSlider.value != food)
            foodSlider.value = food;
    }
}
