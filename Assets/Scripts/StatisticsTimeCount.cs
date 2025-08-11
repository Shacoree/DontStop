using System.Linq;
using TMPro;
using UnityEngine;

public class StatisticsTimeCount : MonoBehaviour
{
    private TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void FixedUpdate()
    {
        text.text = FormatTime(Time.timeSinceLevelLoad);
    }
    private string FormatTime(float timeInSeconds)
    {
        int hours = Mathf.FloorToInt(timeInSeconds / 3600f);
        int minutes = Mathf.FloorToInt((timeInSeconds % 3600f) / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);

        return $"{hours} h {minutes} m {seconds} s";
    }
}
