using UnityEngine;
using UnityEngine.UI;

public class PlayerRageBar : MonoBehaviour
{
    [SerializeField] public Slider rageSlider;
    [SerializeField] public Slider easeRageSlider;
    private float maxRage;
    private float rage;
    private float lerpSpeed = 0.05f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxRage = GameManager.gameManager.playerRage.MaxRage;
    }

    // Update is called once per frame
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
