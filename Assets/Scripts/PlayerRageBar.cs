using UnityEngine;
using UnityEngine.UI;

public class PlayerRageBar : MonoBehaviour
{
    [SerializeField] public Slider rageSlider;
    [SerializeField] public Slider easeRageSlider;
    
    private float maxRage;
    private float rage;
    private float lerpSpeed = 0.05f;

    void Start()
    {
        maxRage = GameManager.gameManager.playerRage.MaxRage;
    }

    void Update()
    {
        rage = GameManager.gameManager.playerRage.Rage;
        if (rageSlider.value != rage)
            rageSlider.value = rage;

        if (rageSlider.value != easeRageSlider.value)
        {
            easeRageSlider.value = Mathf.Lerp(easeRageSlider.value, rage, lerpSpeed);
        }
    }
}
